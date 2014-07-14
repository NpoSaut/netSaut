using System;

namespace Saut.Entities
{
    /// <summary>Положение ограничения</summary>
    /// <remarks>Информация о предстоящем ограничении скорости</remarks>
    public class RestrictionPoint
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public RestrictionPoint(double Disstance)
        {
            this.Disstance = Disstance;
        }

        /// <summary>Расстояние до ограничения</summary>
        public Double Disstance { get; private set; }

        /// <summary>Протяжённость ограничения</summary>
        public Double Length { get; private set; }

        /// <summary>Ограничение скорости на участке</summary>
        public Double Speed { get; private set; }
    }
}
