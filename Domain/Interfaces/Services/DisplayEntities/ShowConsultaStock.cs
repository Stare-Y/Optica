using Domain.Entities;

namespace Domain.Interfaces.Services.DisplayEntities
{
    public class ShowConsultaStock
    {
        public MicaGraduacion MicaGraduacion { get; set; } = null!;
        public int Stock { get; set; }
        public int Taken { get; set; }
        public string BindStockTaken
        {
            get
            {
                return Taken != 0 ? $"{Taken} / {Stock}" : $"{Stock}";
            }
        }
    }
}
