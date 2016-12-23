﻿using System;
using BusinessLogicLayer.Mappers;
using System.Diagnostics;
using GeneralPurposeLib;

namespace BusinessLogicLayer
{
    public partial class LISBLL
    {        
        public IBLL.DTO.RichiestaLISDTO GetRichiestaLISByIdExt(string richidid)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            IBLL.DTO.RichiestaLISDTO rich = null;

            try
            {
                IDAL.VO.RichiestaLISVO rich_ = this.dal.GetRichiestaByIdExt(richidid);
                rich = RichiestaLISMapper.RichMapper(rich_);
                log.Info(string.Format("{0} {1} mapped to {2}", LibString.ItemsNumber(rich), LibString.TypeName(rich_), LibString.TypeName(rich)));
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return rich;
        }
        public IBLL.DTO.RichiestaLISDTO GetRichiestaLISById(string id)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            IBLL.DTO.RichiestaLISDTO rich = null;

            try
            {
                IDAL.VO.RichiestaLISVO rich_ = this.dal.GetRichiestaById(id);
                rich = RichiestaLISMapper.RichMapper(rich_);
                log.Info(string.Format("{0} {1} mapped to {2}", LibString.ItemsNumber(rich), LibString.TypeName(rich_), LibString.TypeName(rich)));
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return rich;
        }
        public IBLL.DTO.RichiestaLISDTO AddRichiestaLIS(IBLL.DTO.RichiestaLISDTO data)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            IBLL.DTO.RichiestaLISDTO toReturn = null;

            try
            {
                data.id = null;
                IDAL.VO.RichiestaLISVO data_ = RichiestaLISMapper.RichMapper(data);
                log.Info(string.Format("{0} {1} mapped to {2}", LibString.ItemsNumber(data_), LibString.TypeName(data), LibString.TypeName(data_)));
                IDAL.VO.RichiestaLISVO stored = dal.NewRichiesta(data_);
                log.Info(string.Format("{0} {1} items added and got back!", LibString.ItemsNumber(stored), LibString.TypeName(stored)));
                toReturn = RichiestaLISMapper.RichMapper(stored);
                log.Info(string.Format("{0} {1} mapped to {2}", LibString.ItemsNumber(toReturn), LibString.TypeName(stored), LibString.TypeName(toReturn)));
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return toReturn;
        }
        public IBLL.DTO.RichiestaLISDTO UpdateRichiestaLIS(IBLL.DTO.RichiestaLISDTO data)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            int stored = 0;
            IBLL.DTO.RichiestaLISDTO toReturn = null;
            string id = data.id.ToString();

            try
            {
                if (id == null || GetRichiestaLISById(id) == null)
                {
                    string msg = string.Format("No record found with the id {0}! Updating is impossible!", id);
                    log.Info(msg);
                    log.Error(msg);
                    return null;
                }
                IDAL.VO.RichiestaLISVO data_ = RichiestaLISMapper.RichMapper(data);
                log.Info(string.Format("{0} {1} mapped to {2}", LibString.ItemsNumber(data_), LibString.TypeName(data), LibString.TypeName(data_)));
                stored = dal.SetRichiesta(data_);
                toReturn = GetRichiestaLISById(id);
                log.Info(string.Format("{0} {1} items added and {2} {3} retrieved back!", stored, LibString.TypeName(data_), LibString.ItemsNumber(toReturn), LibString.TypeName(toReturn)));
            }
            catch (Exception ex)
            {
                string msg = "An Error occured! Exception detected!";
                log.Info(msg);
                log.Error(msg + "\n" + ex.Message);
            }

            tw.Stop();
            log.Info(string.Format("Completed! Elapsed time {0}", LibString.TimeSpanToTimeHmsms(tw.Elapsed)));

            return toReturn;
        }
        public int DeleteRichiestaLISById(string id)
        {
            Stopwatch tw = new Stopwatch();
            tw.Start();

            log.Info(string.Format("Starting ..."));

            int result = 0;

            try
            {
                result = dal.DeleteRichiestaById(id);
                log.Info(string.Format("{0} items Deleted!", result));
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
