using System;

namespace Saut.Entities
{
    /// <summary>Железнодорожная координата</summary>
    public struct RailwayOrdinate : IComparable<RailwayOrdinate>
    {
        /// <summary>Инициализирует новый экземпляр класса <see cref="T:System.Object" />.</summary>
        public RailwayOrdinate(int Kilometer, double Picket = 0, double Meter = 0) : this()
        {
            this.Kilometer = Kilometer;
            this.Picket = (int)Picket;
            this.Meter = Meter;
        }

        /// <summary>Километр</summary>
        public int Kilometer { get; private set; }

        /// <summary>Пикет</summary>
        public int Picket { get; private set; }

        /// <summary>Метр</summary>
        public Double Meter { get; private set; }

        /// <summary>Сравнивает текущий объект с другим объектом того же типа.</summary>
        /// <returns>
        ///     Значение, указывающее, каков относительный порядок сравниваемых объектов. Расшифровка возвращенных значений
        ///     приведена ниже. Значение  Значение  Меньше нуля  Значение этого объекта меньше значения параметра
        ///     <paramref name="other" />. Zero  Значение этого объекта равно значению параметра <paramref name="other" />.  Больше
        ///     нуля.  Значение этого объекта больше значения параметра <paramref name="other" />.
        /// </returns>
        /// <param name="other">Объект, который требуется сравнить с данным объектом.</param>
        public int CompareTo(RailwayOrdinate other)
        {
            if (Kilometer != other.Kilometer) return Kilometer.CompareTo(other.Kilometer);
            if (Picket != other.Picket) return Picket.CompareTo(other.Picket);
            return Meter.CompareTo(other.Meter);
        }

        public static explicit operator Double(RailwayOrdinate Ordinate) { return Ordinate.Kilometer * 1000 + Ordinate.Picket * 100 + Ordinate.Meter; }

        public static explicit operator RailwayOrdinate(Double DoubleOrdinate)
        {
            return new RailwayOrdinate((int)(DoubleOrdinate / 1000), (DoubleOrdinate % 1000) / 100, DoubleOrdinate % 100);
        }

        /// <summary>Возвращает полное имя типа этого экземпляра.</summary>
        /// <returns>Объект типа <see cref="T:System.String" />, содержащий полное имя типа.</returns>
        public override string ToString()
        {
            return string.Format("{0} км {1} пк {2:00} м", Kilometer, Picket, Meter);
        }
    }
}
