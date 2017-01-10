using Seminabit.Sanita.OrderEntry.IBLL.DTO;
using System.Collections.Generic;

namespace Seminabit.Sanita.OrderEntry.ILISPlugin
{
    public interface ILIS
    {
        MirthResponseDTO NewRequest(RichiestaLISDTO rich, List<AnalisiDTO> anals, ref string errorString);

        List<RisultatoDTO> Check4Results(string analid);
        List<AnalisiDTO> Check4Analysis(string richid);
        RefertoDTO Check4Report(string richid);
        List<LabelDTO> Check4Labels(string richid);

        MirthResponseDTO CancelRequest(string richid, ref string errorString);
        
        List<RisultatoDTO> RetrieveResults(string richid, ref string errorString);
    }
}
