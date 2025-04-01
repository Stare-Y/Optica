using Domain.Entities;

namespace Application.ViewModels.Lotes.DisplayHelpers
{
    public class DisplayLoteAndTaken
    {
        public Mica MicaElement;

        public string TakenMicasString = string.Empty;

        public bool showTakenString = false;

        public DisplayLoteAndTaken(Mica mica)
        {
            MicaElement = mica;
        }
    }
}
