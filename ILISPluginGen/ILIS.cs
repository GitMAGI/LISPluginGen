using Seminabit.Sanita.OrderEntry.LIS.IBLL.DTO;
using System.Collections.Generic;

namespace Seminabit.Sanita.OrderEntry.LIS.IPlugin
{
    public interface ILIS
    {
        MirthResponseDTO NewRequest(RichiestaLISDTO rich, List<AnalisiDTO> anals, ref string errorString);

        List<RisultatoDTO> GetResultsByAnalId(string analid);
        List<AnalisiDTO> GetAnalsByRichIdExt(string richid);
        RefertoDTO GetReportByRichIdExt(string richid);
        List<LabelDTO> GetLabelsByRichIdExt(string richid);
        RichiestaLISDTO GetRichiestaByIdExt(string richid);
        List<RichiestaLISDTO> GetRichiesteByEpisodio(string episid);

        MirthResponseDTO CancelRequest(string richid, ref string errorString);
        bool CheckIfCancelingIsAllowed(string richid, ref string errorString);

        List<RisultatoDTO> RetrieveResults(string richid, ref string errorString);
    }
}