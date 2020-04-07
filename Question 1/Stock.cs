using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Question_1
{
    public class Stock : IEnumerable
    { 
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }

        public Stock(string symbol, DateTime date, decimal open, decimal high, decimal low, decimal close)
        {
            Symbol = symbol;
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        public Stock() { }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
