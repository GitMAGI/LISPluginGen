﻿using GeneralPurposeLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.Mappers;
using DBSQLSuite;

namespace Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer
{
    public partial class LISDAL
    {        
        // NO ENTITYFRAMEWORK
        public IDAL.VO.AnalisiVO GetAnalisiById(string analidid)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            IDAL.VO.AnalisiVO anal = null;
            try
            {
                string connectionString = this.GRConnectionString;

                long esamidid_ = long.Parse(analidid);
                string table = this.AnalisiTabName;

                Dictionary<string, DBSQL.QueryCondition> conditions = new Dictionary<string, DBSQL.QueryCondition>()
                {
                    {
                        "id",
                        new DBSQL.QueryCondition() {
                            Key = "analidid",
                            Op = DBSQL.Op.Equal,
                            Value = esamidid_,
                            Conj = DBSQL.Conj.None
                        }
                    }
                };
                DataTable data = DBSQL.SelectOperation(connectionString, table, conditions);
                log.Info(string.Format("DBSQL Query Executed! Retrieved {0} record!", LibString.ItemsNumber(data)));
                if (data != null)
                {
                    if (data.Rows.Count == 1)
                    {
                        anal = AnalisiMapper.AnalMapper(data.Rows[0]);
                        log.Info(string.Format("{0} Records mapped to {1}", LibString.ItemsNumber(anal), LibString.TypeName(anal)));
                    }                    
                }               
            }
            catch (Exception ex)
            {
                log.Info(string.Format("DBSQL Query Executed! Retrieved 0 record!"));
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();

            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return anal;
        }
        public List<IDAL.VO.AnalisiVO> GetAnalisisByIds(List<string> analidids)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            List<IDAL.VO.AnalisiVO> anals = null;
            try
            {
                string connectionString = this.GRConnectionString;

                string table = this.AnalisiTabName;

                Dictionary<string, DBSQL.QueryCondition> conditions = new Dictionary<string, DBSQL.QueryCondition>();
                int i = 0;
                foreach(string analidid in analidids)
                {
                    long esamidid_ = long.Parse(analidid);
                    DBSQL.QueryCondition tmp = new DBSQL.QueryCondition()
                    {
                        Key = "analidid",
                        Op = DBSQL.Op.Equal,
                        Value = esamidid_,
                        Conj = i < analidids.Count-1 ? DBSQL.Conj.Or : DBSQL.Conj.None
                    };
                    conditions.Add("id" + i, tmp);
                    i++;
                }

                DataTable data = DBSQL.SelectOperation(connectionString, table, conditions);
                log.Info(string.Format("DBSQL Query Executed! Retrieved {0} record!", LibString.ItemsNumber(data)));
                if (data != null)
                {
                    if (data.Rows.Count == 1)
                    {
                        anals = AnalisiMapper.AnalMapper(data);
                        log.Info(string.Format("{0} Records mapped to {1}", LibString.ItemsNumber(anals), LibString.TypeName(anals)));
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.Info(string.Format("DBSQL Query Executed! Retrieved 0 record!"));
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();

            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return anals;
        }
        /*
        public List<IDAL.VO.AnalisiVO> GetAnalisisByIdRichiesta(string richidid)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            List<IDAL.VO.AnalisiVO> anals = null;
            try
            {
                string connectionString = this.GRConnectionString;

                long esamidid_ = long.Parse(richidid);
                string table = this.AnalisiTabName;

                Dictionary<string, DBSQL.QueryCondition> conditions = new Dictionary<string, DBSQL.QueryCondition>()
                {
                    {
                        "id",
                        new DBSQL.QueryCondition() {
                            Key = "analesam",
                            Op = DBSQL.Op.Equal,
                            Value = esamidid_,
                            Conj = DBSQL.Conj.None
                        }
                    }
                };
                DataTable data = DBSQL.SelectOperation(connectionString, table, conditions);
                log.Info(string.Format("DBSQL Query Executed! Retrieved {0} record!", LibString.ItemsNumber(data)));
                if (data != null)
                {
                    anals = AnalisiMapper.AnalMapper(data);
                    log.Info(string.Format("{0} Records mapped to {1}", LibString.ItemsNumber(anals), LibString.TypeName(anals)));
                }
            }
            catch (Exception ex)
            {
                log.Info(string.Format("DBSQL Query Executed! Retrieved 0 record!"));
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();

            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return anals;
        }
        */
        public List<IDAL.VO.AnalisiVO> GetAnalisisByIdRichiestaExt(string richididExt)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            List<IDAL.VO.AnalisiVO> anals = null;
            try
            {
                string connectionString = this.GRConnectionString;
                                
                string table = this.AnalisiTabName;

                Dictionary<string, DBSQL.QueryCondition> conditions = new Dictionary<string, DBSQL.QueryCondition>()
                {
                    {
                        "id",
                        new DBSQL.QueryCondition() {
                            Key = "analrich",
                            Op = DBSQL.Op.Equal,
                            Value = richididExt,
                            Conj = DBSQL.Conj.None
                        }
                    }
                };
                DataTable data = DBSQL.SelectOperation(connectionString, table, conditions);
                log.Info(string.Format("DBSQL Query Executed! Retrieved {0} record!", LibString.ItemsNumber(data)));
                if (data != null)
                {
                    anals = AnalisiMapper.AnalMapper(data);
                    log.Info(string.Format("{0} Records mapped to {1}", LibString.ItemsNumber(anals), LibString.TypeName(anals)));
                }
            }
            catch (Exception ex)
            {
                log.Info(string.Format("DBSQL Query Executed! Retrieved 0 record!"));
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();

            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return anals;
        }
        public int SetAnalisi(IDAL.VO.AnalisiVO data)
        {
            int result = 0;

            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            string table = this.AnalisiTabName;

            try
            {
                string connectionString = this.GRConnectionString;
                string analidid = data.analidid.HasValue ? data.analidid.Value.ToString() : null;
                List<string> autoincrement = new List<string>() { "analidid" };

                if (analidid == null)
                {
                    // INSERT NUOVA
                    result = DBSQL.InsertOperation(connectionString, table, data, autoincrement);
                    log.Info(string.Format("Inserted {0} new records!", result));
                }
                else
                {
                    long analidid_ = long.Parse(analidid);                    
                    // UPDATE
                    Dictionary<string, DBSQL.QueryCondition> conditions = new Dictionary<string, DBSQL.QueryCondition>()
                    {
                        { "id",
                            new DBSQL.QueryCondition()
                            {
                                Key = "analidid",
                                Value = analidid_,
                                Op = DBSQL.Op.Equal,
                                Conj = DBSQL.Conj.None,
                            }
                        },
                    };
                    result = DBSQL.UpdateOperation(connectionString, table, data, conditions, new List<string>() { "analidid" });
                    log.Info(string.Format("Updated {0} records!", result));
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

            return result;
        }
        public IDAL.VO.AnalisiVO NewAnalisi(IDAL.VO.AnalisiVO data)
        {
            IDAL.VO.AnalisiVO result = null;

            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            string table = this.AnalisiTabName;

            try
            {
                string connectionString = this.GRConnectionString;

                List<string> pk = new List<string>() { "ANALIDID" };
                List<string> autoincrement = new List<string>() { "aNalIdiD" };
                // INSERT NUOVA
                DataTable res = DBSQL.InsertBackOperation(connectionString, table, data, pk, autoincrement);
                if (res != null)
                    if(res.Rows.Count > 0)
                    {
                        result = AnalisiMapper.AnalMapper(res.Rows[0]);
                        log.Info(string.Format("Inserted new record with ID: {0}!", result.analidid));
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

            return result;
        }
        public List<IDAL.VO.AnalisiVO> NewAnalisi(List<IDAL.VO.AnalisiVO> data)
        {
            List<IDAL.VO.AnalisiVO> results = null;

            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            string table = this.AnalisiTabName;

            try
            {
                string connectionString = this.GRConnectionString;

                List<string> pk = new List<string>() { "ANALIDID" };
                List<string> autoincrement = new List<string>() { "aNalIdiD" };
                // INSERT NUOVA
                DataTable res = DBSQL.MultiInsertBackOperation(connectionString, table, data, pk, autoincrement);
                if (res != null && res.Rows.Count > 0)
                {
                    results = AnalisiMapper.AnalMapper(res);
                }                
                if(results!=null)
                {
                    if (results.Count > 0)
                    {
                        string tmp = "";
                        int o = 0;
                        foreach (IDAL.VO.AnalisiVO tmp_ in results)
                        {
                            tmp += tmp_.analidid.Value.ToString();
                            if (o < results.Count - 1)
                                tmp += ", ";
                            o++;
                        }
                        log.Info(string.Format("Inserted {0} new records with IDs: {1}!", LibString.ItemsNumber(results), tmp));
                    }
                }
                else
                {
                    log.Info(string.Format("No records Inserted!"));
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

            return results;
        }
        public int DeleteAnalisiById(string analidid)
        {
            int result = 0;

            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            string table = this.AnalisiTabName;

            try
            {
                string connectionString = this.GRConnectionString;

                long analidid_ = long.Parse(analidid);
                // UPDATE
                Dictionary<string, DBSQL.QueryCondition> conditions = new Dictionary<string, DBSQL.QueryCondition>()
                    {
                        { "id",
                            new DBSQL.QueryCondition()
                            {
                                Key = "analidid",
                                Value = analidid_,
                                Op = DBSQL.Op.Equal,
                                Conj = DBSQL.Conj.None,
                            }
                        },
                    };
                result = DBSQL.DeleteOperation(connectionString, table, conditions);
                log.Info(string.Format("Deleted {0} records!", result));
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();

            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return result;
        }
        /*
        public int DeleteAnalisiByIdRichiesta(string id)
        {
            int result = 0;

            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            string table = this.AnalisiTabName;

            try
            {
                string connectionString = this.GRConnectionString;

                long id_ = long.Parse(id);
                // UPDATE
                Dictionary<string, DBSQL.QueryCondition> conditions = new Dictionary<string, DBSQL.QueryCondition>()
                    {
                        { "richid",
                            new DBSQL.QueryCondition()
                            {
                                Key = "analesam",
                                Value = id_,
                                Op = DBSQL.Op.Equal,
                                Conj = DBSQL.Conj.None,
                            }
                        },
                    };
                result = DBSQL.DeleteOperation(connectionString, table, conditions);
                log.Info(string.Format("Deleted {0} records!", result));
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();

            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return result;
        }
        */    
        public int DeleteAnalisiByIdRichiestaExt(string richid)
        {
            int result = 0;

            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            string table = this.AnalisiTabName;

            try
            {
                string connectionString = this.GRConnectionString;

                // UPDATE
                Dictionary<string, DBSQL.QueryCondition> conditions = new Dictionary<string, DBSQL.QueryCondition>()
                    {
                        { "id",
                            new DBSQL.QueryCondition()
                            {
                                Key = "analrich",
                                Value = richid,
                                Op = DBSQL.Op.Equal,
                                Conj = DBSQL.Conj.None,
                            }
                        },
                    };
                result = DBSQL.DeleteOperation(connectionString, table, conditions);
                log.Info(string.Format("Deleted {0} records!", result));
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();

            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return result;
        }
    }
}