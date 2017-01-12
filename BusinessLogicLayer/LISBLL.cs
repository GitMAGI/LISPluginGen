namespace Seminabit.Sanita.OrderEntry.LIS.BusinessLogicLayer
{
    public partial class LISBLL : IBLL.ILISBLL
    {
        private static readonly log4net.ILog log =
            //log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.LogManager.GetLogger("LIS");

        private IDAL.ILISDAL dal;

        public LISBLL(IDAL.ILISDAL IDAL)
        {
            this.dal = IDAL;
        }

    }
}
