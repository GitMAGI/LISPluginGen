using System;
using System.Collections.Generic;
using Seminabit.Sanita.OrderEntry.IBLL.DTO;
using System.Diagnostics;
using GeneralPurposeLib;

namespace Seminabit.Sanita.OrderEntry.BusinessLogicLayer
{
    public partial class LISBLL
    {
        public MirthResponseDTO ORLParser(string raw)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            MirthResponseDTO data = new MirthResponseDTO();

            log.Info(string.Format("HL7 Message To Process:\n{0}", raw));

            log.Info(string.Format("HL7 Message Processing ... "));

            try
            {
                
                log.Info(string.Format("MSA Recovering ..."));
                // 1. Get MSA Segment
                string msa = LibString.GetAllValuesSegments(raw, "MSA")[0];
                string[] msaobj = msa.Split('|');
                data.ACKCode = msaobj[1];
                data.MsgID = msaobj[2];
                data.ACKDesc = msaobj.Length > 3 ? msaobj[2] : null;
                switch (data.ACKCode)
                {
                    case "AA":
                        data.Errored = false;
                        data.Accepted = true;
                        data.Refused = false;
                        break;
                    case "AE":
                        data.Errored = true;
                        data.Accepted = false;
                        data.Refused = false;
                        break;
                    case "AR":
                        data.Errored = false;
                        data.Accepted = false;
                        data.Refused = true;
                        break;
                }
                log.Info(string.Format("MSA Recovered"));
                
                // 2. Get ERR Segment            
                log.Info(string.Format("ERR Recovering ..."));
                List<string> errs = LibString.GetAllValuesSegments(raw, "ERR");
                if (errs != null)
                    data.ERRMsg = errs[0];
                log.Info(string.Format("ERR Recovered"));
                // 3. Get ORC Segment
                log.Info(string.Format("ORC Recovering ..."));
                List<string> orcs = LibString.GetAllValuesSegments(raw, "ORC");
                if (orcs != null)
                {
                    foreach (string orc in orcs)
                    {
                        try
                        {
                            string[] ocrobj = orc.Split('|');
                            ORCStatus ORC = new ORCStatus();
                            string[] esIdanId = ocrobj[2].Split('-');
                            ORC.EsamID = esIdanId[0];
                            ORC.AnalID = esIdanId[1];
                            ORC.Status = ocrobj[1];
                            string desc = null;
                            switch (ORC.Status)
                            {
                                case "OK":
                                    desc = "Inserimento/Cancellazione eseguito con successo";
                                    break;
                                case "RQ":
                                    desc = "Modifica Eseguita con successo";
                                    break;
                                case "UA":
                                    desc = "Impossibile Inserire";
                                    break;
                                case "UC":
                                    desc = "Impossibile Cancellare";
                                    break;
                                case "UM":
                                    desc = "Impossibile Modificare";
                                    break;
                            }
                            ORC.Description = desc;
                            if (data.ORCStatus == null)
                                data.ORCStatus = new List<ORCStatus>();
                            data.ORCStatus.Add(ORC);
                        }
                        catch (Exception)
                        {
                            string msg = "Exception During ORC info processing! HL7 Segment errored: " + orc;
                            throw new Exception(msg);
                        }
                    }                    
                }
                log.Info(string.Format("ORC Recovered", data.ORCStatus.Count));
                // 4. Get Labels
                log.Info(string.Format("ZET Recovering ..."));
                List<LabelDTO> labes = Mappers.LabelMapper.LabeMapper(raw);
                data.Labes = labes;
                log.Info(string.Format("ZET Recovered", data.ORCStatus.Count));

                log.Info(string.Format("HL7 Message processing Complete! A DTO object has been built!"));
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }
            finally
            {
                tw.Stop();
                log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));
            }

            return data;
        }
        public string SendMirthRequest(string richidid)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            string data = null;

            try
            {
                // 0. Check if rich exists and labels do
                RichiestaLISDTO rich = GetRichiestaLISByIdExt(richidid);
                if (rich == null)
                {                    
                    string msg = string.Format("An Error occured! No RICH idext {0} found into the DB. Operation Aborted!", richidid);
                    log.Info(msg);
                    log.Error(msg);
                    return null;
                }
                string id = rich.id.Value.ToString();
                log.Info("External Request ID " + richidid + " - Internal Request ID " + id);

                List<AnalisiDTO> anals = GetAnalisisByRichiesta(id);

                if (anals == null || (anals != null && anals.Count == 0))
                {
                    string msg = string.Format("An Error occured! No ANAL related to id {0} found into the DB. Operation Aborted!", id);
                    log.Info(msg);
                    log.Error(msg);
                    return null;
                }
            
                // 1. Call DAL.SendMirthREquest()
                data = this.dal.SendLISRequest(richidid);
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }
            finally
            {
                tw.Stop();
                log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));
            }

            return data;
        }
        public List<LabelDTO> StoreLabels(List<LabelDTO> labes)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            List<LabelDTO> labes_ = AddLabels(labes);

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));
            
            return labes_;
        }
        public int ChangeHL7StatusAndMessageAll(string richidid, string hl7_stato, string hl7_msg = null)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            int res = 0;

            log.Info(string.Format("Starting ..."));

            string msg_ = "Status updating with 'hl7_stato' -> " + hl7_stato;
            if (hl7_msg != null)
                msg_ += " and 'hl7_msg' -> " + hl7_msg;
            log.Info(string.Format(msg_));
            log.Info(string.Format("Updating RICH ..."));

            RichiestaLISDTO got = GetRichiestaLISByIdExt(richidid);

            if (got == null)
            {
                log.Info(string.Format("An Error occurred. Rich bot found! IDExt: {0}", richidid));
                tw.Stop();
                log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));
                return 0;
            }

            string id = got.id.Value.ToString();

            got.hl7_stato = hl7_stato;
            got.hl7_msg = hl7_msg != null ? hl7_msg : got.hl7_msg;
            RichiestaLISDTO updt = UpdateRichiestaLIS(got);

            int richres = 0;
            if (updt != null)
                richres++;
            else
                log.Info(string.Format("An Error occurred. Record not updated! ID: {0}", got.id));
            res = richres;

            log.Info(string.Format("Updated {0}/{1} record!", richres, 1));

            log.Info(string.Format("Updating ANAL ..."));
            List<AnalisiDTO> gots = GetAnalisisByRichiesta(id);
            gots.ForEach(p => { p.hl7_stato = hl7_stato; p.hl7_msg = hl7_msg != null ? hl7_msg : p.hl7_msg; });
            int analsres = 0;
            foreach (AnalisiDTO got_ in gots)
            {
                AnalisiDTO updt_ = UpdateAnalisi(got_);
                if (updt_ != null)
                    analsres++;
                else
                    log.Info(string.Format("An Error occurred. Record not updated! ANALIDID: {0}", got_.analidid));
            }
            res += analsres;
            log.Info(string.Format("Updated {0}/{1} record!", analsres, gots.Count));

            log.Info(string.Format("Updated {0} record overall!", res));

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return res;
        }
        public List<AnalisiDTO> ChangeHL7StatusAndMessageAnalisis(List<string> analidids, string hl7_stato, string hl7_msg = null)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            List<AnalisiDTO> updateds = null;

            log.Info(string.Format("Starting ..."));

            string msg_ = "Status updating with 'hl7_stato' -> " + hl7_stato;
            if (hl7_msg != null)
                msg_ += " and 'hl7_msg' -> " + hl7_msg;
            log.Info(string.Format(msg_));

            log.Info(string.Format("Updating ANAL ..."));
            List<AnalisiDTO> gots = GetAnalisisByIds(analidids);
            gots.ForEach(p => { p.hl7_stato = hl7_stato; p.hl7_msg = hl7_msg != null ? hl7_msg : p.hl7_msg; });
            int analsres = 0;
            foreach (AnalisiDTO got_ in gots)
            {
                AnalisiDTO updt_ = UpdateAnalisi(got_);
                if (updt_ != null)
                {
                    if(updateds==null)
                        updateds = new List<AnalisiDTO>();
                    updateds.Add(updt_);
                    analsres++;
                }                    
                else
                    log.Info(string.Format("An Error occurred. Record not updated! ANALIDID: {0}", got_.analidid));
            }
            log.Info(string.Format("Updated {0}/{1} record!", analsres, gots.Count));

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return updateds;
        }
        public RichiestaLISDTO ChangeHL7StatusAndMessageRichiestaLIS(string richidid, string hl7_stato, string hl7_msg = null)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            RichiestaLISDTO updated = new RichiestaLISDTO();

            log.Info(string.Format("Starting ..."));

            string msg_ = "Status updating with 'hl7_stato' -> " + hl7_stato;
            if (hl7_msg != null)
                msg_ += " and 'hl7_msg' -> " + hl7_msg;
            log.Info(string.Format(msg_));

            log.Info(string.Format("Updating RICH ..."));

            RichiestaLISDTO got = GetRichiestaLISByIdExt(richidid);
            got.hl7_stato = hl7_stato;
            got.hl7_msg = hl7_msg != null ? hl7_msg : got.hl7_msg;
            updated = UpdateRichiestaLIS(got);

            int res = 0;
            if (updated != null)
            {
                res++;                
            }
            else
            {
                log.Info(string.Format("An Error occurred. Record not updated! ID: {0}", got.id));
            }
            log.Info(string.Format("Updated {0}/{1} record!", res, 1));

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return updated;
        }

        public bool ValidateRich(RichiestaLISDTO rich, ref string errorString)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            bool validate = true;            

            if (errorString == null)
                errorString = "";
            /*
            string richid = esam.esamidid.ToString();
            if (esam.esamidid == null)
            {
                string msg = "ESAMIDID is Null!";
                validate = false;
                if(errorString != "")
                    errorString += "\r\n" + "ESAMIDID " + richid + ": " + msg;
                else
                    errorString += "ESAMIDID " + richid + ": " + msg;
            }
            if (esam.esameven == null)
            {
                string msg = "ESAMEVEN is Null!";
                validate = false;
                if (errorString != "")                    
                    errorString += "\r\n" + "ESAM error: " + msg;
                else
                    errorString += "ESAM error: " + msg;
            }
            if(esam.esamtipo == null)
            {
                string msg = "ESAMTIPO is Null!";
                validate = false;
                if (errorString != "")
                    errorString += "\r\n" + "ESAM error: " + msg;
                else
                    errorString += "ESAM error: " + msg;
            }
            if(esam.esampren == null)
            {
                string msg = "ESAMPREN is Null!";
                validate = false;
                if (errorString != "")
                    errorString += "\r\n" + "ESAM error: " + msg;
                else
                    errorString += "ESAM error: " + msg;
            }
            */

            if (errorString == "")
                errorString = null;

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return validate;
        }
        public bool ValidateAnals(List<AnalisiDTO> anals, ref string errorString)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            bool validate = true;

            if (errorString == null)
                errorString = "";
            
            int count = 0;
            foreach (AnalisiDTO anal in anals)
            {
                count++;
                /*
                string analid = anal.analidid.ToString();                
                if (anal.analidid == null)
                {
                    string msg = "";
                    validate = false;                    
                    if (errorString != "")
                        errorString += "\r\n" + "ANALIDID " + analid + ": " + msg;
                    else
                        errorString += "ANALIDID " + analid + ": " + msg;
                }
                */
                if (anal.analesam == null)
                {
                    string msg = "ANALESAM is Null!";
                    validate = false;
                    if (errorString != "")
                        errorString += "\r\n" + "ANAL (" + count + ") error: " + msg;
                    else
                        errorString += "ANAL (" + count + ") error: " + msg;
                }
                if (anal.analcodi == null)
                {
                    string msg = "ANALCODI is Null!";
                    validate = false;
                    if (errorString != "")
                        errorString += "\r\n" + "ANAL (" + count + ") error: " + msg;
                    else
                        errorString += "ANAL (" + count + ") error: " + msg;
                }
                if (anal.analnome == null)
                {
                    string msg = "ANALNOME is Null!";
                    validate = false;
                    if (errorString != "")
                        errorString += "\r\n" + "ANAL (" + count + ") error: " + msg;
                    else
                        errorString += "ANAL (" + count + ") error: " + msg;
                }
                if (anal.analinvi == null)
                {
                    string msg = "ANALINVI is Null!";
                    validate = false;
                    if (errorString != "")
                        errorString += "\r\n" + "ANAL (" + count + ") error: " + msg;
                    else
                        errorString += "ANAL (" + count + ") error: " + msg;
                }
                if (anal.analflro == null)
                {
                    string msg = "ANALFLRO is Null!";
                    validate = false;
                    if (errorString != "")
                        errorString += "\r\n" + "ANAL (" + count + ") error: " + msg;
                    else
                        errorString += "ANAL (" + count + ") error: " + msg;
                }
            }

            if (errorString == "")
                errorString = null;

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return validate;
        }

        public bool StoreNewRequest(RichiestaLISDTO rich, List<AnalisiDTO> anals, ref string errorString)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            string hl7_stato = IBLL.HL7StatesRichiestaLIS.Idle;
            bool stored = true;

            string res = null;

            RichiestaLISDTO richInserted = null;
            List<AnalisiDTO> analsInserted = null;

            if (errorString == null)
                errorString = "";

            try
            {                
                // Validation of Rich!!!!
                if (!this.ValidateRich(rich, ref errorString))
                {
                    string msg = "Validation of Rich Failure! Check the error string for figuring out the issue!";
                    log.Info(msg + "\r\n" + errorString);
                    log.Error(msg + "\r\n" + errorString);
                    throw new Exception(msg);
                }

                rich.hl7_stato = hl7_stato;
                log.Info(string.Format("RICH Insertion ..."));
                richInserted = this.AddRichiestaLIS(rich);
                if (richInserted == null)
                    throw new Exception("Error during RICH writing into the DB.");
                log.Info(string.Format("RICH Inserted. Got {0} ID!", richInserted.id));

                res = richInserted.id.ToString();

                anals.ForEach(p => { p.analesam = int.Parse(res); p.hl7_stato = hl7_stato; });

                // Validation of Anals!!!!
                if (!this.ValidateAnals(anals, ref errorString))
                {
                    string msg = "Validation of Anals Failure! Check the error string for figuring out the issue!";
                    log.Info(msg + "\r\n" + errorString);
                    log.Error(msg + "\r\n" + errorString);
                    throw new Exception(msg);
                }

                log.Info(string.Format("Insertion of {0} ANAL requested. Processing ...", anals.Count));
                analsInserted = this.AddAnalisis(anals);
                if ((analsInserted == null) || (analsInserted != null && analsInserted.Count != anals.Count))
                    throw new Exception("Error during ANALs writing into the DB.");
                log.Info(string.Format("Inserted {0} ANAL successfully!", analsInserted.Count));
                log.Info(string.Format("Inserted {0} records successfully!", analsInserted.Count + 1));
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);

                if (errorString == "")
                    errorString = msg + "\r\n" + ex.Message;
                else
                    errorString += "\r\n" + msg + "\r\n" + ex.Message;

                int richRB = 0;
                int analsRB = 0;

                log.Info(string.Format("Rolling Back of the Insertions due an error occured ..."));
                // Rolling Back
                if (res != null)
                {
                    richRB = this.DeleteRichiestaLISById(res);
                    log.Info(string.Format("Rolled Back {0} RIC record. ESAMIDID was {1}!", richRB, stored));
                    analsRB = this.DeleteAnalisiByIdRichiestaExt(res);
                    log.Info(string.Format("Rolled Back {0} ANAL records. ANALESAM was {1}!", analsRB, stored));
                }
                log.Info(string.Format("Rolled Back {0} records of {1} requested!", richRB + analsRB, anals.Count + 1));
                stored = false;
            }

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            if (errorString == "")
                errorString = null;

            return stored;
        }
        public MirthResponseDTO SubmitNewRequest(string richid, ref string errorString)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            MirthResponseDTO data = null;

            if (errorString == null)
                errorString = "";

            try
            {                
                // 1. Check if Rich and ANAL exist
                RichiestaLISDTO chkRich = this.GetRichiestaLISByIdExt(richid);
                if (chkRich == null)
                {
                    string msg = "Error! No Rich record found referring to IDExt " + richid + "! A request must be Scheduled first!";
                    errorString = msg;
                    log.Info(msg);
                    log.Error(msg);
                    return null;
                }

                string id = chkRich.id.Value.ToString();
                log.Info("External Request ID " + richid + " - Internal Request ID " + id);
                int id_int = 0;
                if (!int.TryParse(id, out id_int))
                {
                    string msg = string.Format("ID of the riquest is not an integer string. {0} is not a valid ID for this context!", id);
                    errorString = msg;
                    log.Info(msg);
                    log.Error(msg);
                    throw new Exception(msg);
                }

                List<AnalisiDTO> chkAnals = this.GetAnalisisByRichiesta(id);
                if (chkAnals == null || (chkAnals != null && chkAnals.Count == 0))
                {
                    string msg = "Error! No Anal records found referring to AnalEsam " + id + "! A request must be Scheduled first!";
                    errorString = msg;
                    log.Info(msg);
                    log.Error(msg);
                    return null;
                }

                // 2. Settare Stato a "SEDNING"
                int res = this.ChangeHL7StatusAndMessageAll(richid, IBLL.HL7StatesRichiestaLIS.Sending, "");

                // 3. Invio a Mirth
                string hl7orl = this.SendMirthRequest(richid);
                if (hl7orl == null)
                {
                    string msg = "Mirth Returned an Error!";
                    errorString = msg;
                    // 3.e1 Cambiare stato in errato
                    int err = this.ChangeHL7StatusAndMessageAll(richid, IBLL.HL7StatesRichiestaLIS.Errored, msg);
                    // 3.e2 Restituire null
                    return null;
                }
                // 3.1 Settare a SENT
                int snt = this.ChangeHL7StatusAndMessageAll(richid, IBLL.HL7StatesRichiestaLIS.Sent, "");

                // 4. Estrarre i dati dalla risposta di Mirth                
                log.Info("Mirth Data Response Extraction ...");
                data = this.ORLParser(hl7orl);
                if (data == null)
                {
                    string emsg = "Mirth Data Response Extraction failed!";
                    if (errorString == "")
                        errorString = emsg;
                    else
                        errorString += "\n\r" + emsg;
                    log.Info(emsg);
                    log.Error(emsg);

                }
                else
                    log.Info("Mirth Data Response Successfully extracted!");

                // 5. Settare Stato a seconda della risposta
                string status = IBLL.HL7StatesRichiestaLIS.Sent;
                if (data.ACKCode != "AA")
                    status = IBLL.HL7StatesRichiestaLIS.Errored;
                else
                {
                    if (data.Labes != null)
                    {
                        status = IBLL.HL7StatesRichiestaLIS.Labelled;
                    }
                    else
                    {
                        string msg = "An Error Occurred! No Lables Retrieved By the Remote LAB!";
                        errorString = msg;
                        log.Info(msg);
                        log.Error(msg);
                        return null;
                    }
                }
                RichiestaLISDTO RichUpdt = this.ChangeHL7StatusAndMessageRichiestaLIS(richid, status, data.ACKDesc);

                List<ORCStatus> orcs = data.ORCStatus;
                if (orcs != null)
                    foreach (ORCStatus orc in orcs)
                    {
                        string desc = orc.Description;
                        string stat = orc.Status;
                        string analid = orc.AnalID;
                        List<AnalisiDTO> AnalUpdts = this.ChangeHL7StatusAndMessageAnalisis(new List<string>() { analid }, stat, desc);
                    }

                // 6. Scrivere Labels nel DB
                if (data.Labes != null)
                {
                    data.Labes.ForEach(p => p.labeesam = id_int);
                    List<LabelDTO> stored = this.StoreLabels(data.Labes);
                    if (stored == null)
                    {
                        string msg = "An Error Occurred! Labels successfully retrieved by the remote LAB, but they haven't been stored into the local DB! The Rich ID is " + id;
                        errorString = msg;
                        log.Info(msg);
                        log.Error(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            if (errorString == "")
                errorString = null;

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            // 7. Restituire il DTO
            return data;
        }

        public bool CheckIfCancelingIsAllowed(string richid, ref string errorString)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            bool res = true;

            if (errorString == null)
                errorString = "";

            RichiestaLISDTO rich = this.GetRichiestaLISByIdExt(richid);           

            if (rich == null)
            {
                if (errorString == "")
                    errorString = null;

                string msg = string.Format("Error! No Rich found with IDExt: {0}", richid);

                log.Info(string.Format(msg));
                log.Error(string.Format(msg));

                if (errorString != "")
                    errorString += "\r\n" + msg;
                else
                    errorString += msg;

                tw.Stop();
                log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

                return false;
            }

            string id = rich.id.Value.ToString();
            log.Info("External Request ID " + richid + " - Internal Request ID " + id);

            RefertoDTO refe = this.GetRefertoByEsamId(id);

            if (refe == null)
            {
                List<AnalisiDTO> anals = this.GetAnalisisByRichiesta(id);
                foreach (AnalisiDTO anal in anals)
                {
                    List<RisultatoDTO> riss = this.GetRisultatiByAnalId(anal.analidid.Value.ToString());
                    if (riss != null)
                    {
                        string report = string.Format("Analisi {0} già eseguita! Impossibile Cancellare!", anal.analidid.Value.ToString());
                        res = false;
                        if (errorString != "")
                            errorString += "\r\n" + report;
                        else
                            errorString += report;
                    }
                }
            }
            else
            {
                string report = string.Format("Rich {0} (IDExt: {1}) già refertato! Id referto {2}!", id, richid, refe.refeidid);
                res = false;
                if (errorString != "")
                    errorString += "\r\n" + report;
                else
                    errorString += report;
            }

            if (errorString == "")
                errorString = null;

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return res;
        }
    }
}
