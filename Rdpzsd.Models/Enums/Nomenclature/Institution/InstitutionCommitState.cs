using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Nomenclature.Institution
{
    public enum InstitutionCommitState
    {
        [Description("първоначална чернова")]
        InitialDraft = 1,
        [Description("в процес на промяна")]
        Modification,
        [Description("актуално състояние")]
        Actual,
        [Description("актуално с инициирана промяна")]
        ActualWithModification,
        [Description("предишно състояние")]
        History,
        [Description("заличено")]
        Deleted,
        [Description("готов за вписване")]
        CommitReady,
        [Description("заявено за заличаване")]
        ModificationErase,
        [Description("Заявено за възстановяване")]
        ModificationRevertErase
    }
}
