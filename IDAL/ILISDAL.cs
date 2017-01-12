using System;
using System.Collections.Generic;

namespace Seminabit.Sanita.OrderEntry.LIS.IDAL
{
    public interface ILISDAL
    {
        VO.PazienteVO GetPazienteById(string pazidid);
        List<VO.PazienteVO> GetPazienteBy5IdentityFields(string pazicogn, string pazinome, string pazisess, DateTime pazidata, string pazicofi);
        int SetPaziente(VO.PazienteVO data);
        VO.PazienteVO NewPaziente(VO.PazienteVO data);
        int DeletePazienteById(string pazidid);

        List<VO.RichiestaLISVO> GetRichiesteByEpisodio(string episid);
        VO.RichiestaLISVO GetRichiestaById(string id);
        VO.RichiestaLISVO GetRichiestaByIdExt(string richidid);
        int SetRichiesta(VO.RichiestaLISVO data);
        VO.RichiestaLISVO NewRichiesta(VO.RichiestaLISVO data);
        int DeleteRichiestaById(string id);

        VO.AnalisiVO GetAnalisiById(string analidid);
        List<VO.AnalisiVO> GetAnalisisByIds(List<string> analidids);
        //List<VO.AnalisiVO> GetAnalisisByIdRichiesta(string richidid);
        List<VO.AnalisiVO> GetAnalisisByIdRichiestaExt(string richidid);
        int SetAnalisi(VO.AnalisiVO data);
        List<VO.AnalisiVO> NewAnalisi(List<VO.AnalisiVO> data);
        VO.AnalisiVO NewAnalisi(VO.AnalisiVO data);
        int DeleteAnalisiById(string analidid);
        //int DeleteAnalisiByIdRichiesta(string id);
        int DeleteAnalisiByIdRichiestaExt(string richidid);

        VO.LabelVO GetLabelById(string labeidid);
        //List<VO.LabelVO> GetLabelsByIdRichiesta(string richidid);
        List<VO.LabelVO> GetLabelsByIdRichiestaExt(string richidid);
        int SetLabel(VO.LabelVO data);
        List<VO.LabelVO> NewLabels(List<VO.LabelVO> data);
        VO.LabelVO NewLabel(VO.LabelVO data);
        int DeleteLabelById(string labeidid);
        int DeleteLabelByRichiesta(string labeidid);

        string SendLISRequest(string richidid);

        VO.RisultatoGrezzoVO GetRisultatoGrezzoByEsamAnalId(string id);
        VO.RisultatoGrezzoVO GetRisultatoGrezzoById(string id);
        VO.RisultatoVO GetRisultatoById(string id);
        List<VO.RisultatoVO> GetRisultatiByAnalId(string id);
        VO.RisultatoVO NewRisultato(VO.RisultatoVO data);
        List<VO.RisultatoVO> NewRisultati(List<VO.RisultatoVO> data);

        VO.RefertoVO GetRefertoById(string refeidid);
        //VO.RefertoVO GetRefertoByIdRichiesta(string esamidid);
        VO.RefertoVO GetRefertoByIdRichiestaExt(string esamidid);
        int SetReferto(VO.RefertoVO data);
        int DeleteRefertoById(string refeidid);
    }
}