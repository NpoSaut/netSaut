using System;

namespace Saut.Entities
{
    /// <summary>Положение ограничения</summary>
    /// <remarks>Информация о предстоящем ограничении скорости</remarks>
    public class RestrictionTarget
    {
        public RestrictionTarget(double Disstance, double Speed, double Length = 0)
        {
            this.Length = Length;
            this.Speed = Speed;
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
