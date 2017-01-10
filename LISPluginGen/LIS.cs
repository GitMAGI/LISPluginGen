using GeneralPurposeLib;
using Seminabit.Sanita.OrderEntry.IBLL.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seminabit.Sanita.OrderEntry;

namespace Seminabit.Sanita.OrderEntry.LISPlugin
{
    public class LIS : ILISPlugin.ILIS
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DataAccessLayer.LISDAL dal;
        private BusinessLogicLayer.LISBLL bll;

        public LIS()
        {
            dal = new DataAccessLayer.LISDAL();
            bll = new BusinessLogicLayer.LISBLL(dal);
        }
                
        public MirthResponseDTO NewRequest(RichiestaLISDTO rich, List<AnalisiDTO> anals,  ref string errorString)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            MirthResponseDTO data = null;

            if (errorString == null)
                errorString = "";

            try
            {
                if(!bll.StoreNewRequest(rich, anals, ref errorString))
                {
                    throw new Exception(errorString);
                }
                data = bll.SubmitNewRequest(rich.richidid, ref errorString);
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
                        
            return data;
        }

        public List<RisultatoDTO> Check4Results(string analid)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            List<RisultatoDTO> riss = null;

            riss = bll.GetRisultatiByAnalId(analid);

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return riss;
        }
        public List<AnalisiDTO> Check4Analysis(string richid_)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            List<AnalisiDTO> anals = null;

            RichiestaLISDTO rich = bll.GetRichiestaLISByIdExt(richid_);
            if (rich != null)
            {
                string richid = rich.id.ToString();
                log.Info("External Request ID " + richid_ + " - Internal Request ID " + richid);
                anals = bll.GetAnalisisByRichiesta(richid);                
            }
            else
            {
                string msg = string.Format("No Rich with ID: {0} found. The operation will be aborted!", richid_);
                log.Info(msg);
            }            

            rich = null;

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return anals;
        }        
        public RefertoDTO Check4Report(string richid_)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            RefertoDTO refe = null;

            RichiestaLISDTO rich = bll.GetRichiestaLISById(richid_);
            if (rich != null)
            {
                string richid = rich.id.ToString();
                log.Info("External Request ID " + richid_ + " - Internal Request ID " + richid);
                refe = bll.GetRefertoByEsamId(richid);                
            }
            else
            {
                string msg = string.Format("No Rich with ID: {0} found. The operation will be aborted!", richid_);
                log.Info(msg);
            }
            
            rich = null;            

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return refe;
        }
        public List<LabelDTO> Check4Labels(string richid_)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            List<LabelDTO> labes = null;

            RichiestaLISDTO rich = bll.GetRichiestaLISById(richid_);
            if (rich != null)
            {
                string richid = rich.id.ToString();
                log.Info("External Request ID " + richid_ + " - Internal Request ID " + richid);
                labes = bll.GetLabelsByRichiesta(richid);                
            }
            else
            {
                string msg = string.Format("No Rich with ID: {0} found. The operation will be aborted!", richid_);
                log.Info(msg);
            }            

            rich = null;

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return labes;
        }

        public MirthResponseDTO CancelRequest(string richid_, ref string errorString)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            MirthResponseDTO data = null;

            try
            {                
                // 1. Check if Canceling is allowed
                if (bll.CheckIfCancelingIsAllowed(richid_, ref errorString))
                {
                    string msg = string.Format("Canceling of the request with id {0} is denied! errorString: {1}", richid_, errorString);
                    log.Info(msg);
                    log.Error(msg);
                    throw new Exception(msg);
                }

                // 2. Check if ESAM and ANAL exist
                RichiestaLISDTO chkEsam = bll.GetRichiestaLISById(richid_);
                List<AnalisiDTO> chkAnals = bll.GetAnalisisByRichiesta(richid_);
                if (chkEsam == null || chkAnals == null || (chkAnals != null && chkAnals.Count == 0))
                {
                    string msg = "Error! No Esam or Anal records found referring to EsamID " + richid_ + "! A request must be Scheduled first!";
                    errorString = msg;
                    log.Info(msg);
                    log.Error(msg);
                    return null;
                }

                // 3. Settare Stato a "DELETNG"
                int res = bll.ChangeHL7StatusAndMessageAll(richid_, IBLL.HL7StatesRichiestaLIS.Deleting);

                // 4. Invio a Mirth
                string hl7orl = bll.SendMirthRequest(richid_);
                if (hl7orl == null)
                {
                    string msg = "Mirth Returned an Error!";
                    errorString = msg;
                    // 4.e1 Cambiare stato in errato
                    int err = bll.ChangeHL7StatusAndMessageAll(richid_, IBLL.HL7StatesRichiestaLIS.Errored, msg);
                    // 4.e2 Restituire null
                    return null;
                }

                // 5. Estrarre i dati dalla risposta di Mirth
                data = bll.ORLParser(hl7orl);

                // 6. Settare Stato a seconda della risposta
                string status = IBLL.HL7StatesRichiestaLIS.Deleted;
                if (data.ACKCode != "AA")
                    status = IBLL.HL7StatesRichiestaLIS.Errored;
                RichiestaLISDTO RichUpdt = bll.ChangeHL7StatusAndMessageRichiestaLIS(richid_, status, data.ACKDesc);

                List<ORCStatus> orcs = data.ORCStatus;
                if (orcs != null)
                    foreach (ORCStatus orc in orcs)
                    {
                        string desc = orc.Description;
                        string stat = orc.Status;
                        string analid = orc.AnalID;
                        List<AnalisiDTO> AnalUpdts = bll.ChangeHL7StatusAndMessageAnalisis(new List<string>() { analid }, stat, desc);
                    }
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return data;
        }
        
        public List<RisultatoDTO> RetrieveResults(string richid_, ref string errorString)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            List<RisultatoDTO> riss = null;

            try
            {
                RichiestaLISDTO rich = bll.GetRichiestaLISById(richid_);
                if (rich == null)
                {
                    string msg = string.Format("No Rich with ID: {0} found. The operation will be aborted!", richid_);
                    log.Info(msg);
                    throw new Exception(msg);
                }

                string richid = rich.id.ToString();
                rich = null;

                log.Info("External Request ID " + richid_ + " - Internal Request ID " + richid); 

                log.Info("Searching for Analysis' related to Request ID " + richid + " ...");
                List<AnalisiDTO> anals = bll.GetAnalisisByRichiesta(richid);
                log.Info(string.Format("Found {0} Analysis' related to Request ID {1}.", anals != null ? anals.Count : 0, richid));
                foreach (AnalisiDTO anal in anals)
                {
                    log.Info("Searching for Results related to Analysis ID " + anal.analidid.Value.ToString() + " ...");
                    List<RisultatoDTO> anres = bll.GetRisultatiByAnalId(anal.analidid.Value.ToString());
                    if (anres != null && anres.Count > 0)
                    {
                        log.Info(string.Format("Found {0} Results related to Analysis ID {1}.", anres, anal.analidid.Value.ToString()));
                        //1. Check if Analisi is "Executed"
                        //1.1 If not, Update Analisi to "Executed"
                        log.Info(string.Format("HL7 Status of Analysis with ID {0}, is '{1}'. HL7 Message is '{2}'.", anal.analidid.Value.ToString(), anal.hl7_stato, anal.hl7_msg));
                        if (anal.hl7_stato != IBLL.HL7StatesAnalisi.Executed)
                        {
                            List<AnalisiDTO> tmp = bll.ChangeHL7StatusAndMessageAnalisis(new List<string>() { anal.analidid.Value.ToString() }, IBLL.HL7StatesAnalisi.Executed, "Risultati Ottenuti");
                            log.Info(string.Format("HL7 Status of Analysis with ID {0}, has been updated to '{1}'.", anal.analidid.Value.ToString(), tmp != null ? tmp.First().hl7_stato : "--error occurred--"));
                        }
                        //2. Add to Collection
                        if (riss == null)
                            riss = new List<RisultatoDTO>();
                        riss.AddRange(anres);
                        log.Info(string.Format("{0} Results related to Analysis ID {1}, has been added to the Results Collection (actually {2} total items).", anres.Count, anal.analidid.Value.ToString(), riss.Count));
                    }
                    else
                    {
                        log.Info(string.Format("Found No Results related to Analysis ID {0}.", anal.analidid.Value.ToString()));
                        log.Info("Searching for Raw Results related to Request ID - Analysis ID " + richid + "-" + anal.analidid.Value.ToString() + " ...");
                        List<RisultatoDTO> anresNew = bll.GetRisultatiByEsamAnalId(richid + "-" + anal.analidid.Value.ToString());
                        log.Info(string.Format("Found {0} Raw Results related to Request ID - Analysis ID : {1}-{2}.", anresNew != null ? anresNew.Count : 0, richid, anal.analidid.Value.ToString()));
                        if (anresNew != null && anresNew.Count > 0)
                        {
                            //1. Add new Risultato as Executed                            
                            List<RisultatoDTO> news = bll.AddRisultati(anresNew);
                            log.Info(string.Format("{0} Raw Results Converted and Written into DB. They are Related to Analysis ID {1}. ANRE ID are '{2}'.", news != null ? news.Count : 0, anal.analidid.Value.ToString(), news != null ? string.Join(", ", news.Select(p => p.anreidid).ToList().ToArray()) : ""));
                            //2. Update Analisi to "Executed"                            
                            log.Info(string.Format("HL7 Status of Analysis with ID {0}, is '{1}'. HL7 Message is '{2}'.", anal.analidid.Value.ToString(), anal.hl7_stato, anal.hl7_msg));
                            List<AnalisiDTO> tmp = bll.ChangeHL7StatusAndMessageAnalisis(new List<string>() { anal.analidid.Value.ToString() }, IBLL.HL7StatesAnalisi.Executed, "Risultati Ottenuti");
                            log.Info(string.Format("HL7 Status of Analysis with ID {0}, has been updated to '{1}'.", anal.analidid.Value.ToString(), tmp != null ? tmp.First().hl7_stato : "--error occurred--"));
                            //3. Add to Collection                        
                            if (news != null && news.Count > 0)
                            {
                                if (riss == null)
                                    riss = new List<RisultatoDTO>();
                                riss.AddRange(news);
                                log.Info(string.Format("{0} Results related to Analysis ID {1}, has been added to the Results Collection (actually {2} total items).", news.Count, anal.analidid.Value.ToString(), riss.Count));
                            }
                        }
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

            return riss;
        }
    }
}
