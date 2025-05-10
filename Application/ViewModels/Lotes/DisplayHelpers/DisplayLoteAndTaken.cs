using Domain.Entities;

namespace Application.ViewModels.Lotes.DisplayHelpers
{
    public class DisplayLoteAndTaken
    {
        public Mica MicaElement { get; set; }

        public string TakenMicasString { get; set; } = string.Empty;

        public bool showTakenString { get; set; } = false;

        public DisplayLoteAndTaken(Mica mica)
        {
            MicaElement = mica;
        }
    }
}
