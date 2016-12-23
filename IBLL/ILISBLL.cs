﻿using System.Collections.Generic;

namespace IBLL
{
    public interface ILISBLL
    {
        DTO.PazienteDTO GetPazienteById(string id);
        
        DTO.RichiestaLISDTO GetRichiestaLISById(string id);
        DTO.RichiestaLISDTO GetRichiestaLISByIdExt(string richidid);
        DTO.RichiestaLISDTO AddRichiestaLIS(DTO.RichiestaLISDTO rich);
        DTO.RichiestaLISDTO UpdateRichiestaLIS(DTO.RichiestaLISDTO rich);
        int DeleteRichiestaLISById(string id);

        List<DTO.AnalisiDTO> GetAnalisisByRichiesta(string richidid);
        DTO.AnalisiDTO GetAnalisiById(string analidid);
        List<DTO.AnalisiDTO> GetAnalisisByIds(List<string> analidids);
        DTO.AnalisiDTO UpdateAnalisi(DTO.AnalisiDTO data);
        DTO.AnalisiDTO AddAnalisi(DTO.AnalisiDTO data);
        List<DTO.AnalisiDTO> AddAnalisis(List<DTO.AnalisiDTO> data);
        int DeleteAnalisiById(string analidid);
        int DeleteAnalisiByRichiesta(string richidid);

        List<DTO.LabelDTO> GetLabelsByRichiesta(string richidid);
        DTO.LabelDTO GetLabelById(string labeidid);
        DTO.LabelDTO UpdatLabel(DTO.LabelDTO data);
        DTO.LabelDTO AddLabel(DTO.LabelDTO data);
        List<DTO.LabelDTO> AddLabels(List<DTO.LabelDTO> data);
        int DeleteLabelById(string labeidid);

        List<DTO.RisultatoDTO> GetRisultatiByEsamAnalId(string id);
        List<DTO.RisultatoDTO> GetRisultatiByAnalId(string id);
        List<DTO.RisultatoDTO> AddRisultati(List<DTO.RisultatoDTO> data);

        DTO.MirthResponseDTO ORLParser(string raw);
        string SendMirthRequest(string richidid);
        List<DTO.LabelDTO> StoreLabels(List<DTO.LabelDTO> labes);
        int ChangeHL7StatusAndMessageAll(string richidid, string hl7_stato, string hl7_msg = null);
        List<DTO.AnalisiDTO> ChangeHL7StatusAndMessageAnalisis(List<string> analidids, string hl7_stato, string hl7_msg = null);
        DTO.RichiestaLISDTO ChangeHL7StatusAndMessageRichiestaLIS(string richidid, string hl7_stato, string hl7_msg = null);
        bool ValidateRich(DTO.RichiestaLISDTO esam, ref string errorString);
        bool ValidateAnals(List<DTO.AnalisiDTO> anals, ref string errorString);

        bool StoreNewRequest(DTO.RichiestaLISDTO rich, List<DTO.AnalisiDTO> anals, ref string errorString);
        DTO.MirthResponseDTO SubmitNewRequest(string richid, ref string errorString);

        bool CheckIfCancelingIsAllowed(string richid, ref string errorString);

        DTO.RefertoDTO GetRefertoByEsamId(string id);
        DTO.RefertoDTO GetRefertoById(string id);
    }
}