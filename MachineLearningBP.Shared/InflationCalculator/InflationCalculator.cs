using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Abp.Timing;

namespace MachineLearningBP.Shared.InflationCalculator
{
    public class InflationCalculator : IInflationCalculator
    {
        #region Properties
        private static readonly String codeGenPath = "C:\\Users\\csandfort\\Documents\\Visual Studio 2017\\Projects\\MachineLearningBP\\MachineLearningBP.Shared\\CodeGen\\";
        readonly IConsoleHubProxy _consoleHubProxy;

        Dictionary<DateTime, Double> _cpiDictionary = new Dictionary<DateTime, Double>();
        Dictionary<int, Double> _theaterCountDictionary = new Dictionary<int, Double>();
        #endregion

        #region Constructor
        public InflationCalculator(IConsoleHubProxy consoleHubProxy)
        {
            this._consoleHubProxy = consoleHubProxy;

            #region InitCpiDictionary
            this._cpiDictionary.Add(new DateTime(1980, 1, 1), 77.8);
            this._cpiDictionary.Add(new DateTime(1980, 2, 1), 78.9);
            this._cpiDictionary.Add(new DateTime(1980, 3, 1), 80.1);
            this._cpiDictionary.Add(new DateTime(1980, 4, 1), 81);
            this._cpiDictionary.Add(new DateTime(1980, 5, 1), 81.8);
            this._cpiDictionary.Add(new DateTime(1980, 6, 1), 82.7);
            this._cpiDictionary.Add(new DateTime(1980, 7, 1), 82.7);
            this._cpiDictionary.Add(new DateTime(1980, 8, 1), 83.3);
            this._cpiDictionary.Add(new DateTime(1980, 9, 1), 84);
            this._cpiDictionary.Add(new DateTime(1980, 10, 1), 84.8);
            this._cpiDictionary.Add(new DateTime(1980, 11, 1), 85.5);
            this._cpiDictionary.Add(new DateTime(1980, 12, 1), 86.3);
            this._cpiDictionary.Add(new DateTime(1981, 1, 1), 87);
            this._cpiDictionary.Add(new DateTime(1981, 2, 1), 87.9);
            this._cpiDictionary.Add(new DateTime(1981, 3, 1), 88.5);
            this._cpiDictionary.Add(new DateTime(1981, 4, 1), 89.1);
            this._cpiDictionary.Add(new DateTime(1981, 5, 1), 89.8);
            this._cpiDictionary.Add(new DateTime(1981, 6, 1), 90.6);
            this._cpiDictionary.Add(new DateTime(1981, 7, 1), 91.6);
            this._cpiDictionary.Add(new DateTime(1981, 8, 1), 92.3);
            this._cpiDictionary.Add(new DateTime(1981, 9, 1), 93.2);
            this._cpiDictionary.Add(new DateTime(1981, 10, 1), 93.4);
            this._cpiDictionary.Add(new DateTime(1981, 11, 1), 93.7);
            this._cpiDictionary.Add(new DateTime(1981, 12, 1), 94);
            this._cpiDictionary.Add(new DateTime(1982, 1, 1), 94.3);
            this._cpiDictionary.Add(new DateTime(1982, 2, 1), 94.6);
            this._cpiDictionary.Add(new DateTime(1982, 3, 1), 94.5);
            this._cpiDictionary.Add(new DateTime(1982, 4, 1), 94.9);
            this._cpiDictionary.Add(new DateTime(1982, 5, 1), 95.8);
            this._cpiDictionary.Add(new DateTime(1982, 6, 1), 97);
            this._cpiDictionary.Add(new DateTime(1982, 7, 1), 97.5);
            this._cpiDictionary.Add(new DateTime(1982, 8, 1), 97.7);
            this._cpiDictionary.Add(new DateTime(1982, 9, 1), 97.9);
            this._cpiDictionary.Add(new DateTime(1982, 10, 1), 98.2);
            this._cpiDictionary.Add(new DateTime(1982, 11, 1), 98);
            this._cpiDictionary.Add(new DateTime(1982, 12, 1), 97.6);
            this._cpiDictionary.Add(new DateTime(1983, 1, 1), 97.8);
            this._cpiDictionary.Add(new DateTime(1983, 2, 1), 97.9);
            this._cpiDictionary.Add(new DateTime(1983, 3, 1), 97.9);
            this._cpiDictionary.Add(new DateTime(1983, 4, 1), 98.6);
            this._cpiDictionary.Add(new DateTime(1983, 5, 1), 99.2);
            this._cpiDictionary.Add(new DateTime(1983, 6, 1), 99.5);
            this._cpiDictionary.Add(new DateTime(1983, 7, 1), 99.9);
            this._cpiDictionary.Add(new DateTime(1983, 8, 1), 100.2);
            this._cpiDictionary.Add(new DateTime(1983, 9, 1), 100.7);
            this._cpiDictionary.Add(new DateTime(1983, 10, 1), 101);
            this._cpiDictionary.Add(new DateTime(1983, 11, 1), 101.2);
            this._cpiDictionary.Add(new DateTime(1983, 12, 1), 101.3);
            this._cpiDictionary.Add(new DateTime(1984, 1, 1), 101.9);
            this._cpiDictionary.Add(new DateTime(1984, 2, 1), 102.4);
            this._cpiDictionary.Add(new DateTime(1984, 3, 1), 102.6);
            this._cpiDictionary.Add(new DateTime(1984, 4, 1), 103.1);
            this._cpiDictionary.Add(new DateTime(1984, 5, 1), 103.4);
            this._cpiDictionary.Add(new DateTime(1984, 6, 1), 103.7);
            this._cpiDictionary.Add(new DateTime(1984, 7, 1), 104.1);
            this._cpiDictionary.Add(new DateTime(1984, 8, 1), 104.5);
            this._cpiDictionary.Add(new DateTime(1984, 9, 1), 105);
            this._cpiDictionary.Add(new DateTime(1984, 10, 1), 105.3);
            this._cpiDictionary.Add(new DateTime(1984, 11, 1), 105.3);
            this._cpiDictionary.Add(new DateTime(1984, 12, 1), 105.3);
            this._cpiDictionary.Add(new DateTime(1985, 1, 1), 105.5);
            this._cpiDictionary.Add(new DateTime(1985, 2, 1), 106);
            this._cpiDictionary.Add(new DateTime(1985, 3, 1), 106.4);
            this._cpiDictionary.Add(new DateTime(1985, 4, 1), 106.9);
            this._cpiDictionary.Add(new DateTime(1985, 5, 1), 107.3);
            this._cpiDictionary.Add(new DateTime(1985, 6, 1), 107.6);
            this._cpiDictionary.Add(new DateTime(1985, 7, 1), 107.8);
            this._cpiDictionary.Add(new DateTime(1985, 8, 1), 108);
            this._cpiDictionary.Add(new DateTime(1985, 9, 1), 108.3);
            this._cpiDictionary.Add(new DateTime(1985, 10, 1), 108.7);
            this._cpiDictionary.Add(new DateTime(1985, 11, 1), 109);
            this._cpiDictionary.Add(new DateTime(1985, 12, 1), 109.3);
            this._cpiDictionary.Add(new DateTime(1986, 1, 1), 109.6);
            this._cpiDictionary.Add(new DateTime(1986, 2, 1), 109.3);
            this._cpiDictionary.Add(new DateTime(1986, 3, 1), 108.8);
            this._cpiDictionary.Add(new DateTime(1986, 4, 1), 108.6);
            this._cpiDictionary.Add(new DateTime(1986, 5, 1), 108.9);
            this._cpiDictionary.Add(new DateTime(1986, 6, 1), 109.5);
            this._cpiDictionary.Add(new DateTime(1986, 7, 1), 109.5);
            this._cpiDictionary.Add(new DateTime(1986, 8, 1), 109.7);
            this._cpiDictionary.Add(new DateTime(1986, 9, 1), 110.2);
            this._cpiDictionary.Add(new DateTime(1986, 10, 1), 110.3);
            this._cpiDictionary.Add(new DateTime(1986, 11, 1), 110.4);
            this._cpiDictionary.Add(new DateTime(1986, 12, 1), 110.5);
            this._cpiDictionary.Add(new DateTime(1987, 1, 1), 111.2);
            this._cpiDictionary.Add(new DateTime(1987, 2, 1), 111.6);
            this._cpiDictionary.Add(new DateTime(1987, 3, 1), 112.1);
            this._cpiDictionary.Add(new DateTime(1987, 4, 1), 112.7);
            this._cpiDictionary.Add(new DateTime(1987, 5, 1), 113.1);
            this._cpiDictionary.Add(new DateTime(1987, 6, 1), 113.5);
            this._cpiDictionary.Add(new DateTime(1987, 7, 1), 113.8);
            this._cpiDictionary.Add(new DateTime(1987, 8, 1), 114.4);
            this._cpiDictionary.Add(new DateTime(1987, 9, 1), 115);
            this._cpiDictionary.Add(new DateTime(1987, 10, 1), 115.3);
            this._cpiDictionary.Add(new DateTime(1987, 11, 1), 115.4);
            this._cpiDictionary.Add(new DateTime(1987, 12, 1), 115.4);
            this._cpiDictionary.Add(new DateTime(1988, 1, 1), 115.7);
            this._cpiDictionary.Add(new DateTime(1988, 2, 1), 116);
            this._cpiDictionary.Add(new DateTime(1988, 3, 1), 116.5);
            this._cpiDictionary.Add(new DateTime(1988, 4, 1), 117.1);
            this._cpiDictionary.Add(new DateTime(1988, 5, 1), 117.5);
            this._cpiDictionary.Add(new DateTime(1988, 6, 1), 118);
            this._cpiDictionary.Add(new DateTime(1988, 7, 1), 118.5);
            this._cpiDictionary.Add(new DateTime(1988, 8, 1), 119);
            this._cpiDictionary.Add(new DateTime(1988, 9, 1), 119.8);
            this._cpiDictionary.Add(new DateTime(1988, 10, 1), 120.2);
            this._cpiDictionary.Add(new DateTime(1988, 11, 1), 120.3);
            this._cpiDictionary.Add(new DateTime(1988, 12, 1), 120.5);
            this._cpiDictionary.Add(new DateTime(1989, 1, 1), 121.1);
            this._cpiDictionary.Add(new DateTime(1989, 2, 1), 121.6);
            this._cpiDictionary.Add(new DateTime(1989, 3, 1), 122.3);
            this._cpiDictionary.Add(new DateTime(1989, 4, 1), 123.1);
            this._cpiDictionary.Add(new DateTime(1989, 5, 1), 123.8);
            this._cpiDictionary.Add(new DateTime(1989, 6, 1), 124.1);
            this._cpiDictionary.Add(new DateTime(1989, 7, 1), 124.4);
            this._cpiDictionary.Add(new DateTime(1989, 8, 1), 124.6);
            this._cpiDictionary.Add(new DateTime(1989, 9, 1), 125);
            this._cpiDictionary.Add(new DateTime(1989, 10, 1), 125.6);
            this._cpiDictionary.Add(new DateTime(1989, 11, 1), 125.9);
            this._cpiDictionary.Add(new DateTime(1989, 12, 1), 126.1);
            this._cpiDictionary.Add(new DateTime(1990, 1, 1), 127.4);
            this._cpiDictionary.Add(new DateTime(1990, 2, 1), 128);
            this._cpiDictionary.Add(new DateTime(1990, 3, 1), 128.7);
            this._cpiDictionary.Add(new DateTime(1990, 4, 1), 128.9);
            this._cpiDictionary.Add(new DateTime(1990, 5, 1), 129.2);
            this._cpiDictionary.Add(new DateTime(1990, 6, 1), 129.9);
            this._cpiDictionary.Add(new DateTime(1990, 7, 1), 130.4);
            this._cpiDictionary.Add(new DateTime(1990, 8, 1), 131.6);
            this._cpiDictionary.Add(new DateTime(1990, 9, 1), 132.7);
            this._cpiDictionary.Add(new DateTime(1990, 10, 1), 133.5);
            this._cpiDictionary.Add(new DateTime(1990, 11, 1), 133.8);
            this._cpiDictionary.Add(new DateTime(1990, 12, 1), 133.8);
            this._cpiDictionary.Add(new DateTime(1991, 1, 1), 134.6);
            this._cpiDictionary.Add(new DateTime(1991, 2, 1), 134.8);
            this._cpiDictionary.Add(new DateTime(1991, 3, 1), 135);
            this._cpiDictionary.Add(new DateTime(1991, 4, 1), 135.2);
            this._cpiDictionary.Add(new DateTime(1991, 5, 1), 135.6);
            this._cpiDictionary.Add(new DateTime(1991, 6, 1), 136);
            this._cpiDictionary.Add(new DateTime(1991, 7, 1), 136.2);
            this._cpiDictionary.Add(new DateTime(1991, 8, 1), 136.6);
            this._cpiDictionary.Add(new DateTime(1991, 9, 1), 137.2);
            this._cpiDictionary.Add(new DateTime(1991, 10, 1), 137.4);
            this._cpiDictionary.Add(new DateTime(1991, 11, 1), 137.8);
            this._cpiDictionary.Add(new DateTime(1991, 12, 1), 137.9);
            this._cpiDictionary.Add(new DateTime(1992, 1, 1), 138.1);
            this._cpiDictionary.Add(new DateTime(1992, 2, 1), 138.6);
            this._cpiDictionary.Add(new DateTime(1992, 3, 1), 139.3);
            this._cpiDictionary.Add(new DateTime(1992, 4, 1), 139.5);
            this._cpiDictionary.Add(new DateTime(1992, 5, 1), 139.7);
            this._cpiDictionary.Add(new DateTime(1992, 6, 1), 140.2);
            this._cpiDictionary.Add(new DateTime(1992, 7, 1), 140.5);
            this._cpiDictionary.Add(new DateTime(1992, 8, 1), 140.9);
            this._cpiDictionary.Add(new DateTime(1992, 9, 1), 141.3);
            this._cpiDictionary.Add(new DateTime(1992, 10, 1), 141.8);
            this._cpiDictionary.Add(new DateTime(1992, 11, 1), 142);
            this._cpiDictionary.Add(new DateTime(1992, 12, 1), 141.9);
            this._cpiDictionary.Add(new DateTime(1993, 1, 1), 142.6);
            this._cpiDictionary.Add(new DateTime(1993, 2, 1), 143.1);
            this._cpiDictionary.Add(new DateTime(1993, 3, 1), 143.6);
            this._cpiDictionary.Add(new DateTime(1993, 4, 1), 144);
            this._cpiDictionary.Add(new DateTime(1993, 5, 1), 144.2);
            this._cpiDictionary.Add(new DateTime(1993, 6, 1), 144.4);
            this._cpiDictionary.Add(new DateTime(1993, 7, 1), 144.4);
            this._cpiDictionary.Add(new DateTime(1993, 8, 1), 144.8);
            this._cpiDictionary.Add(new DateTime(1993, 9, 1), 145.1);
            this._cpiDictionary.Add(new DateTime(1993, 10, 1), 145.7);
            this._cpiDictionary.Add(new DateTime(1993, 11, 1), 145.8);
            this._cpiDictionary.Add(new DateTime(1993, 12, 1), 145.8);
            this._cpiDictionary.Add(new DateTime(1994, 1, 1), 146.2);
            this._cpiDictionary.Add(new DateTime(1994, 2, 1), 146.7);
            this._cpiDictionary.Add(new DateTime(1994, 3, 1), 147.2);
            this._cpiDictionary.Add(new DateTime(1994, 4, 1), 147.4);
            this._cpiDictionary.Add(new DateTime(1994, 5, 1), 147.5);
            this._cpiDictionary.Add(new DateTime(1994, 6, 1), 148);
            this._cpiDictionary.Add(new DateTime(1994, 7, 1), 148.4);
            this._cpiDictionary.Add(new DateTime(1994, 8, 1), 149);
            this._cpiDictionary.Add(new DateTime(1994, 9, 1), 149.4);
            this._cpiDictionary.Add(new DateTime(1994, 10, 1), 149.5);
            this._cpiDictionary.Add(new DateTime(1994, 11, 1), 149.7);
            this._cpiDictionary.Add(new DateTime(1994, 12, 1), 149.7);
            this._cpiDictionary.Add(new DateTime(1995, 1, 1), 150.3);
            this._cpiDictionary.Add(new DateTime(1995, 2, 1), 150.9);
            this._cpiDictionary.Add(new DateTime(1995, 3, 1), 151.4);
            this._cpiDictionary.Add(new DateTime(1995, 4, 1), 151.9);
            this._cpiDictionary.Add(new DateTime(1995, 5, 1), 152.2);
            this._cpiDictionary.Add(new DateTime(1995, 6, 1), 152.5);
            this._cpiDictionary.Add(new DateTime(1995, 7, 1), 152.5);
            this._cpiDictionary.Add(new DateTime(1995, 8, 1), 152.9);
            this._cpiDictionary.Add(new DateTime(1995, 9, 1), 153.2);
            this._cpiDictionary.Add(new DateTime(1995, 10, 1), 153.7);
            this._cpiDictionary.Add(new DateTime(1995, 11, 1), 153.6);
            this._cpiDictionary.Add(new DateTime(1995, 12, 1), 153.5);
            this._cpiDictionary.Add(new DateTime(1996, 1, 1), 154.4);
            this._cpiDictionary.Add(new DateTime(1996, 2, 1), 154.9);
            this._cpiDictionary.Add(new DateTime(1996, 3, 1), 155.7);
            this._cpiDictionary.Add(new DateTime(1996, 4, 1), 156.3);
            this._cpiDictionary.Add(new DateTime(1996, 5, 1), 156.6);
            this._cpiDictionary.Add(new DateTime(1996, 6, 1), 156.7);
            this._cpiDictionary.Add(new DateTime(1996, 7, 1), 157);
            this._cpiDictionary.Add(new DateTime(1996, 8, 1), 157.3);
            this._cpiDictionary.Add(new DateTime(1996, 9, 1), 157.8);
            this._cpiDictionary.Add(new DateTime(1996, 10, 1), 158.3);
            this._cpiDictionary.Add(new DateTime(1996, 11, 1), 158.6);
            this._cpiDictionary.Add(new DateTime(1996, 12, 1), 158.6);
            this._cpiDictionary.Add(new DateTime(1997, 1, 1), 159.1);
            this._cpiDictionary.Add(new DateTime(1997, 2, 1), 159.6);
            this._cpiDictionary.Add(new DateTime(1997, 3, 1), 160);
            this._cpiDictionary.Add(new DateTime(1997, 4, 1), 160.2);
            this._cpiDictionary.Add(new DateTime(1997, 5, 1), 160.1);
            this._cpiDictionary.Add(new DateTime(1997, 6, 1), 160.3);
            this._cpiDictionary.Add(new DateTime(1997, 7, 1), 160.5);
            this._cpiDictionary.Add(new DateTime(1997, 8, 1), 160.8);
            this._cpiDictionary.Add(new DateTime(1997, 9, 1), 161.2);
            this._cpiDictionary.Add(new DateTime(1997, 10, 1), 161.6);
            this._cpiDictionary.Add(new DateTime(1997, 11, 1), 161.5);
            this._cpiDictionary.Add(new DateTime(1997, 12, 1), 161.3);
            this._cpiDictionary.Add(new DateTime(1998, 1, 1), 161.6);
            this._cpiDictionary.Add(new DateTime(1998, 2, 1), 161.9);
            this._cpiDictionary.Add(new DateTime(1998, 3, 1), 162.2);
            this._cpiDictionary.Add(new DateTime(1998, 4, 1), 162.5);
            this._cpiDictionary.Add(new DateTime(1998, 5, 1), 162.8);
            this._cpiDictionary.Add(new DateTime(1998, 6, 1), 163);
            this._cpiDictionary.Add(new DateTime(1998, 7, 1), 163.2);
            this._cpiDictionary.Add(new DateTime(1998, 8, 1), 163.4);
            this._cpiDictionary.Add(new DateTime(1998, 9, 1), 163.6);
            this._cpiDictionary.Add(new DateTime(1998, 10, 1), 164);
            this._cpiDictionary.Add(new DateTime(1998, 11, 1), 164);
            this._cpiDictionary.Add(new DateTime(1998, 12, 1), 163.9);
            this._cpiDictionary.Add(new DateTime(1999, 1, 1), 164.3);
            this._cpiDictionary.Add(new DateTime(1999, 2, 1), 164.5);
            this._cpiDictionary.Add(new DateTime(1999, 3, 1), 165);
            this._cpiDictionary.Add(new DateTime(1999, 4, 1), 166.2);
            this._cpiDictionary.Add(new DateTime(1999, 5, 1), 166.2);
            this._cpiDictionary.Add(new DateTime(1999, 6, 1), 166.2);
            this._cpiDictionary.Add(new DateTime(1999, 7, 1), 166.7);
            this._cpiDictionary.Add(new DateTime(1999, 8, 1), 167.1);
            this._cpiDictionary.Add(new DateTime(1999, 9, 1), 167.9);
            this._cpiDictionary.Add(new DateTime(1999, 10, 1), 168.2);
            this._cpiDictionary.Add(new DateTime(1999, 11, 1), 168.3);
            this._cpiDictionary.Add(new DateTime(1999, 12, 1), 168.3);
            this._cpiDictionary.Add(new DateTime(2000, 1, 1), 168.8);
            this._cpiDictionary.Add(new DateTime(2000, 2, 1), 169.8);
            this._cpiDictionary.Add(new DateTime(2000, 3, 1), 171.2);
            this._cpiDictionary.Add(new DateTime(2000, 4, 1), 171.3);
            this._cpiDictionary.Add(new DateTime(2000, 5, 1), 171.5);
            this._cpiDictionary.Add(new DateTime(2000, 6, 1), 172.4);
            this._cpiDictionary.Add(new DateTime(2000, 7, 1), 172.8);
            this._cpiDictionary.Add(new DateTime(2000, 8, 1), 172.8);
            this._cpiDictionary.Add(new DateTime(2000, 9, 1), 173.7);
            this._cpiDictionary.Add(new DateTime(2000, 10, 1), 174);
            this._cpiDictionary.Add(new DateTime(2000, 11, 1), 174.1);
            this._cpiDictionary.Add(new DateTime(2000, 12, 1), 174);
            this._cpiDictionary.Add(new DateTime(2001, 1, 1), 175.1);
            this._cpiDictionary.Add(new DateTime(2001, 2, 1), 175.8);
            this._cpiDictionary.Add(new DateTime(2001, 3, 1), 176.2);
            this._cpiDictionary.Add(new DateTime(2001, 4, 1), 176.9);
            this._cpiDictionary.Add(new DateTime(2001, 5, 1), 177.7);
            this._cpiDictionary.Add(new DateTime(2001, 6, 1), 178);
            this._cpiDictionary.Add(new DateTime(2001, 7, 1), 177.5);
            this._cpiDictionary.Add(new DateTime(2001, 8, 1), 177.5);
            this._cpiDictionary.Add(new DateTime(2001, 9, 1), 178.3);
            this._cpiDictionary.Add(new DateTime(2001, 10, 1), 177.7);
            this._cpiDictionary.Add(new DateTime(2001, 11, 1), 177.4);
            this._cpiDictionary.Add(new DateTime(2001, 12, 1), 176.7);
            this._cpiDictionary.Add(new DateTime(2002, 1, 1), 177.1);
            this._cpiDictionary.Add(new DateTime(2002, 2, 1), 177.8);
            this._cpiDictionary.Add(new DateTime(2002, 3, 1), 178.8);
            this._cpiDictionary.Add(new DateTime(2002, 4, 1), 179.8);
            this._cpiDictionary.Add(new DateTime(2002, 5, 1), 179.8);
            this._cpiDictionary.Add(new DateTime(2002, 6, 1), 179.9);
            this._cpiDictionary.Add(new DateTime(2002, 7, 1), 180.1);
            this._cpiDictionary.Add(new DateTime(2002, 8, 1), 180.7);
            this._cpiDictionary.Add(new DateTime(2002, 9, 1), 181);
            this._cpiDictionary.Add(new DateTime(2002, 10, 1), 181.3);
            this._cpiDictionary.Add(new DateTime(2002, 11, 1), 181.3);
            this._cpiDictionary.Add(new DateTime(2002, 12, 1), 180.9);
            this._cpiDictionary.Add(new DateTime(2003, 1, 1), 181.7);
            this._cpiDictionary.Add(new DateTime(2003, 2, 1), 183.1);
            this._cpiDictionary.Add(new DateTime(2003, 3, 1), 184.2);
            this._cpiDictionary.Add(new DateTime(2003, 4, 1), 183.8);
            this._cpiDictionary.Add(new DateTime(2003, 5, 1), 183.5);
            this._cpiDictionary.Add(new DateTime(2003, 6, 1), 183.7);
            this._cpiDictionary.Add(new DateTime(2003, 7, 1), 183.9);
            this._cpiDictionary.Add(new DateTime(2003, 8, 1), 184.6);
            this._cpiDictionary.Add(new DateTime(2003, 9, 1), 185.2);
            this._cpiDictionary.Add(new DateTime(2003, 10, 1), 185);
            this._cpiDictionary.Add(new DateTime(2003, 11, 1), 184.5);
            this._cpiDictionary.Add(new DateTime(2003, 12, 1), 184.3);
            this._cpiDictionary.Add(new DateTime(2004, 1, 1), 185.2);
            this._cpiDictionary.Add(new DateTime(2004, 2, 1), 186.2);
            this._cpiDictionary.Add(new DateTime(2004, 3, 1), 187.4);
            this._cpiDictionary.Add(new DateTime(2004, 4, 1), 188);
            this._cpiDictionary.Add(new DateTime(2004, 5, 1), 189.1);
            this._cpiDictionary.Add(new DateTime(2004, 6, 1), 189.7);
            this._cpiDictionary.Add(new DateTime(2004, 7, 1), 189.4);
            this._cpiDictionary.Add(new DateTime(2004, 8, 1), 189.5);
            this._cpiDictionary.Add(new DateTime(2004, 9, 1), 189.9);
            this._cpiDictionary.Add(new DateTime(2004, 10, 1), 190.9);
            this._cpiDictionary.Add(new DateTime(2004, 11, 1), 191);
            this._cpiDictionary.Add(new DateTime(2004, 12, 1), 190.3);
            this._cpiDictionary.Add(new DateTime(2005, 1, 1), 190.7);
            this._cpiDictionary.Add(new DateTime(2005, 2, 1), 191.8);
            this._cpiDictionary.Add(new DateTime(2005, 3, 1), 193.3);
            this._cpiDictionary.Add(new DateTime(2005, 4, 1), 194.6);
            this._cpiDictionary.Add(new DateTime(2005, 5, 1), 194.4);
            this._cpiDictionary.Add(new DateTime(2005, 6, 1), 194.5);
            this._cpiDictionary.Add(new DateTime(2005, 7, 1), 195.4);
            this._cpiDictionary.Add(new DateTime(2005, 8, 1), 196.4);
            this._cpiDictionary.Add(new DateTime(2005, 9, 1), 198.8);
            this._cpiDictionary.Add(new DateTime(2005, 10, 1), 199.2);
            this._cpiDictionary.Add(new DateTime(2005, 11, 1), 197.6);
            this._cpiDictionary.Add(new DateTime(2005, 12, 1), 196.8);
            this._cpiDictionary.Add(new DateTime(2006, 1, 1), 198.3);
            this._cpiDictionary.Add(new DateTime(2006, 2, 1), 198.7);
            this._cpiDictionary.Add(new DateTime(2006, 3, 1), 199.8);
            this._cpiDictionary.Add(new DateTime(2006, 4, 1), 201.5);
            this._cpiDictionary.Add(new DateTime(2006, 5, 1), 202.5);
            this._cpiDictionary.Add(new DateTime(2006, 6, 1), 202.9);
            this._cpiDictionary.Add(new DateTime(2006, 7, 1), 203.5);
            this._cpiDictionary.Add(new DateTime(2006, 8, 1), 203.9);
            this._cpiDictionary.Add(new DateTime(2006, 9, 1), 202.9);
            this._cpiDictionary.Add(new DateTime(2006, 10, 1), 201.8);
            this._cpiDictionary.Add(new DateTime(2006, 11, 1), 201.5);
            this._cpiDictionary.Add(new DateTime(2006, 12, 1), 201.8);
            this._cpiDictionary.Add(new DateTime(2007, 1, 1), 202.4);
            this._cpiDictionary.Add(new DateTime(2007, 2, 1), 203.5);
            this._cpiDictionary.Add(new DateTime(2007, 3, 1), 205.4);
            this._cpiDictionary.Add(new DateTime(2007, 4, 1), 206.7);
            this._cpiDictionary.Add(new DateTime(2007, 5, 1), 207.9);
            this._cpiDictionary.Add(new DateTime(2007, 6, 1), 208.4);
            this._cpiDictionary.Add(new DateTime(2007, 7, 1), 208.3);
            this._cpiDictionary.Add(new DateTime(2007, 8, 1), 207.9);
            this._cpiDictionary.Add(new DateTime(2007, 9, 1), 208.5);
            this._cpiDictionary.Add(new DateTime(2007, 10, 1), 208.9);
            this._cpiDictionary.Add(new DateTime(2007, 11, 1), 210.2);
            this._cpiDictionary.Add(new DateTime(2007, 12, 1), 210);
            this._cpiDictionary.Add(new DateTime(2008, 1, 1), 211.1);
            this._cpiDictionary.Add(new DateTime(2008, 2, 1), 211.7);
            this._cpiDictionary.Add(new DateTime(2008, 3, 1), 213.5);
            this._cpiDictionary.Add(new DateTime(2008, 4, 1), 214.8);
            this._cpiDictionary.Add(new DateTime(2008, 5, 1), 216.6);
            this._cpiDictionary.Add(new DateTime(2008, 6, 1), 218.8);
            this._cpiDictionary.Add(new DateTime(2008, 7, 1), 219.964);
            this._cpiDictionary.Add(new DateTime(2008, 8, 1), 219.086);
            this._cpiDictionary.Add(new DateTime(2008, 9, 1), 218.783);
            this._cpiDictionary.Add(new DateTime(2008, 10, 1), 216.573);
            this._cpiDictionary.Add(new DateTime(2008, 11, 1), 212.425);
            this._cpiDictionary.Add(new DateTime(2008, 12, 1), 210.228);
            this._cpiDictionary.Add(new DateTime(2009, 1, 1), 211.143);
            this._cpiDictionary.Add(new DateTime(2009, 2, 1), 212.193);
            this._cpiDictionary.Add(new DateTime(2009, 3, 1), 212.709);
            this._cpiDictionary.Add(new DateTime(2009, 4, 1), 213.24);
            this._cpiDictionary.Add(new DateTime(2009, 5, 1), 213.856);
            this._cpiDictionary.Add(new DateTime(2009, 6, 1), 215.693);
            this._cpiDictionary.Add(new DateTime(2009, 7, 1), 215.351);
            this._cpiDictionary.Add(new DateTime(2009, 8, 1), 215.834);
            this._cpiDictionary.Add(new DateTime(2009, 9, 1), 215.969);
            this._cpiDictionary.Add(new DateTime(2009, 10, 1), 216.177);
            this._cpiDictionary.Add(new DateTime(2009, 11, 1), 216.33);
            this._cpiDictionary.Add(new DateTime(2009, 12, 1), 215.949);
            this._cpiDictionary.Add(new DateTime(2010, 1, 1), 216.687);
            this._cpiDictionary.Add(new DateTime(2010, 2, 1), 216.741);
            this._cpiDictionary.Add(new DateTime(2010, 3, 1), 217.631);
            this._cpiDictionary.Add(new DateTime(2010, 4, 1), 218.009);
            this._cpiDictionary.Add(new DateTime(2010, 5, 1), 218.178);
            this._cpiDictionary.Add(new DateTime(2010, 6, 1), 217.965);
            this._cpiDictionary.Add(new DateTime(2010, 7, 1), 218.011);
            this._cpiDictionary.Add(new DateTime(2010, 8, 1), 218.312);
            this._cpiDictionary.Add(new DateTime(2010, 9, 1), 218.439);
            this._cpiDictionary.Add(new DateTime(2010, 10, 1), 218.711);
            this._cpiDictionary.Add(new DateTime(2010, 11, 1), 218.803);
            this._cpiDictionary.Add(new DateTime(2010, 12, 1), 219.179);
            this._cpiDictionary.Add(new DateTime(2011, 1, 1), 220.223);
            this._cpiDictionary.Add(new DateTime(2011, 2, 1), 221.309);
            this._cpiDictionary.Add(new DateTime(2011, 3, 1), 223.467);
            this._cpiDictionary.Add(new DateTime(2011, 4, 1), 224.906);
            this._cpiDictionary.Add(new DateTime(2011, 5, 1), 225.964);
            this._cpiDictionary.Add(new DateTime(2011, 6, 1), 225.722);
            this._cpiDictionary.Add(new DateTime(2011, 7, 1), 225.922);
            this._cpiDictionary.Add(new DateTime(2011, 8, 1), 226.545);
            this._cpiDictionary.Add(new DateTime(2011, 9, 1), 226.889);
            this._cpiDictionary.Add(new DateTime(2011, 10, 1), 226.421);
            this._cpiDictionary.Add(new DateTime(2011, 11, 1), 226.23);
            this._cpiDictionary.Add(new DateTime(2011, 12, 1), 225.672);
            this._cpiDictionary.Add(new DateTime(2012, 1, 1), 226.655);
            this._cpiDictionary.Add(new DateTime(2012, 2, 1), 227.663);
            this._cpiDictionary.Add(new DateTime(2012, 3, 1), 229.392);
            this._cpiDictionary.Add(new DateTime(2012, 4, 1), 230.085);
            this._cpiDictionary.Add(new DateTime(2012, 5, 1), 229.815);
            this._cpiDictionary.Add(new DateTime(2012, 6, 1), 229.478);
            this._cpiDictionary.Add(new DateTime(2012, 7, 1), 229.104);
            this._cpiDictionary.Add(new DateTime(2012, 8, 1), 230.379);
            this._cpiDictionary.Add(new DateTime(2012, 9, 1), 231.407);
            this._cpiDictionary.Add(new DateTime(2012, 10, 1), 231.317);
            this._cpiDictionary.Add(new DateTime(2012, 11, 1), 230.221);
            this._cpiDictionary.Add(new DateTime(2012, 12, 1), 229.601);
            this._cpiDictionary.Add(new DateTime(2013, 1, 1), 230.28);
            this._cpiDictionary.Add(new DateTime(2013, 2, 1), 232.166);
            this._cpiDictionary.Add(new DateTime(2013, 3, 1), 232.773);
            this._cpiDictionary.Add(new DateTime(2013, 4, 1), 232.531);
            this._cpiDictionary.Add(new DateTime(2013, 5, 1), 232.945);
            this._cpiDictionary.Add(new DateTime(2013, 6, 1), 233.504);
            this._cpiDictionary.Add(new DateTime(2013, 7, 1), 233.596);
            this._cpiDictionary.Add(new DateTime(2013, 8, 1), 233.877);
            this._cpiDictionary.Add(new DateTime(2013, 9, 1), 234.149);
            this._cpiDictionary.Add(new DateTime(2013, 10, 1), 233.546);
            this._cpiDictionary.Add(new DateTime(2013, 11, 1), 233.069);
            this._cpiDictionary.Add(new DateTime(2013, 12, 1), 233.049);
            this._cpiDictionary.Add(new DateTime(2014, 1, 1), 233.916);
            this._cpiDictionary.Add(new DateTime(2014, 2, 1), 234.781);
            this._cpiDictionary.Add(new DateTime(2014, 3, 1), 236.293);
            this._cpiDictionary.Add(new DateTime(2014, 4, 1), 237.072);
            this._cpiDictionary.Add(new DateTime(2014, 5, 1), 237.9);
            this._cpiDictionary.Add(new DateTime(2014, 6, 1), 238.343);
            this._cpiDictionary.Add(new DateTime(2014, 7, 1), 238.25);
            this._cpiDictionary.Add(new DateTime(2014, 8, 1), 237.852);
            this._cpiDictionary.Add(new DateTime(2014, 9, 1), 238.031);
            this._cpiDictionary.Add(new DateTime(2014, 10, 1), 237.433);
            this._cpiDictionary.Add(new DateTime(2014, 11, 1), 236.151);
            this._cpiDictionary.Add(new DateTime(2014, 12, 1), 234.812);
            this._cpiDictionary.Add(new DateTime(2015, 1, 1), 233.707);
            this._cpiDictionary.Add(new DateTime(2015, 2, 1), 234.722);
            this._cpiDictionary.Add(new DateTime(2015, 3, 1), 236.119);
            this._cpiDictionary.Add(new DateTime(2015, 4, 1), 236.599);
            this._cpiDictionary.Add(new DateTime(2015, 5, 1), 237.805);
            this._cpiDictionary.Add(new DateTime(2015, 6, 1), 238.638);
            this._cpiDictionary.Add(new DateTime(2015, 7, 1), 238.654);
            this._cpiDictionary.Add(new DateTime(2015, 8, 1), 238.316);
            this._cpiDictionary.Add(new DateTime(2015, 9, 1), 237.945);
            this._cpiDictionary.Add(new DateTime(2015, 10, 1), 237.838);
            this._cpiDictionary.Add(new DateTime(2015, 11, 1), 237.336);
            this._cpiDictionary.Add(new DateTime(2015, 12, 1), 236.525);
            this._cpiDictionary.Add(new DateTime(2016, 1, 1), 236.916);
            this._cpiDictionary.Add(new DateTime(2016, 2, 1), 237.111);
            this._cpiDictionary.Add(new DateTime(2016, 3, 1), 238.132);
            this._cpiDictionary.Add(new DateTime(2016, 4, 1), 239.261);
            this._cpiDictionary.Add(new DateTime(2016, 5, 1), 240.236);
            this._cpiDictionary.Add(new DateTime(2016, 6, 1), 241.038);
            this._cpiDictionary.Add(new DateTime(2016, 7, 1), 240.647);
            this._cpiDictionary.Add(new DateTime(2016, 8, 1), 240.853);
            this._cpiDictionary.Add(new DateTime(2016, 9, 1), 241.428);
            this._cpiDictionary.Add(new DateTime(2016, 10, 1), 241.729);
            this._cpiDictionary.Add(new DateTime(2016, 11, 1), 241.353);
            this._cpiDictionary.Add(new DateTime(2016, 12, 1), 241.432);
            this._cpiDictionary.Add(new DateTime(2017, 1, 1), 242.839);
            this._cpiDictionary.Add(new DateTime(2017, 2, 1), 243.603);
            this._cpiDictionary.Add(new DateTime(2017, 3, 1), 243.801);
            this._cpiDictionary.Add(new DateTime(2017, 4, 1), 244.0);
            #endregion

            #region InitTheaterCountDictionary
            this._theaterCountDictionary.Add(1985, 0.423043055104973);
            this._theaterCountDictionary.Add(1986, 0.418825601334755);
            this._theaterCountDictionary.Add(1987, 0.430597395374705);
            this._theaterCountDictionary.Add(1988, 0.518097974695278);
            this._theaterCountDictionary.Add(1989, 0.524308291236038);
            this._theaterCountDictionary.Add(1990, 0.553459702460954);
            this._theaterCountDictionary.Add(1991, 0.570051443666868);
            this._theaterCountDictionary.Add(1992, 0.571024702229226);
            this._theaterCountDictionary.Add(1993, 0.55012281596144);
            this._theaterCountDictionary.Add(1994, 0.607127960328127);
            this._theaterCountDictionary.Add(1995, 0.626778514158595);
            this._theaterCountDictionary.Add(1996, 0.661723131111832);
            this._theaterCountDictionary.Add(1997, 0.700977893126941);
            this._theaterCountDictionary.Add(1998, 0.742318209204245);
            this._theaterCountDictionary.Add(1999, 0.753719238077583);
            this._theaterCountDictionary.Add(2000, 0.794874171571581);
            this._theaterCountDictionary.Add(2001, 0.815405292672754);
            this._theaterCountDictionary.Add(2002, 0.838346387356908);
            this._theaterCountDictionary.Add(2003, 0.846178801501599);
            this._theaterCountDictionary.Add(2004, 0.932428048384854);
            this._theaterCountDictionary.Add(2005, 0.905593919451268);
            this._theaterCountDictionary.Add(2006, 0.940631227696158);
            this._theaterCountDictionary.Add(2007, 0.974741623024517);
            this._theaterCountDictionary.Add(2008, 0.968670343421236);
            this._theaterCountDictionary.Add(2009, 0.966816517588173);
            this._theaterCountDictionary.Add(2010, 0.991194327292951);
            this._theaterCountDictionary.Add(2011, 0.963711359317792);
            this._theaterCountDictionary.Add(2012, 1);
            this._theaterCountDictionary.Add(2013, 0.958103536172777);
            this._theaterCountDictionary.Add(2014, 0.975158733836956);
            this._theaterCountDictionary.Add(2015, 0.980673865690318);
            this._theaterCountDictionary.Add(2016, 0.991704129397043);
            this._theaterCountDictionary.Add(2017, 0.951244380590444);
            #endregion
        }
        #endregion

        #region CalculateCpi
        public double CalculateCpi(double val, DateTime from, DateTime to)
        {
            if (from.Month == Clock.Now.Month && from.Year == Clock.Now.Year)
                from = from.AddMonths(-1);

            from = new DateTime(from.Year, from.Month, 1);
            to = new DateTime(to.Year, to.Month, 1);

            return val * (this._cpiDictionary[to] / this._cpiDictionary[from]);
        }
        #endregion

        #region GenCpiDictionary
        public void GenCpiDictionary()
        {
            HtmlDocument doc = new HtmlDocument();
            HtmlWeb getHtml = new HtmlWeb();

            doc = getHtml.Load("http://www.usinflationcalculator.com/inflation/consumer-price-index-and-annual-percent-changes-from-1913-to-2008/");

            HtmlNode cpiTable = doc.DocumentNode.QuerySelector("table");
            List<HtmlNode> columns;
            int year;
            Double cpi;

            using (StreamWriter dictFile = new StreamWriter($"{codeGenPath}dict.txt", false))
            {
                dictFile.WriteLine("#region InitCpiDictionary");

                foreach (HtmlNode row in cpiTable.QuerySelectorAll("tbody > tr").Skip(2))
                {
                    columns = row.Elements("td").ToList();
                    year = Int32.Parse(columns[0].Element("strong").InnerText);

                    if (year >= 1980)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            if (Double.TryParse(columns[i].InnerText.Trim(), out cpi))
                            {
                                dictFile.WriteLine($"this._cpiDictionary.Add(new DateTime({year}, {i} ,1), {cpi});");
                            }
                        } 
                    }
                }

                dictFile.WriteLine("#endregion");
                dictFile.Close();
            }
        }
        #endregion

        #region GenTheaterCountDictionary
        public void GenTheaterCountDictionary(List<KeyValuePair<int, double>> theaterCounts)
        {
            Double max = theaterCounts.Max(x => x.Value);

            using (StreamWriter dictFile = new StreamWriter($"{codeGenPath}theaterCountDict.txt", false))
            {
                dictFile.WriteLine("#region InitTheaterCountDictionary");

                foreach(KeyValuePair<int, double> theaterCount in theaterCounts)
                {
                    dictFile.WriteLine($"this._theaterCountDictionary.Add({theaterCount.Key}, {theaterCount.Value / max});");
                }

                dictFile.WriteLine("#endregion");
                dictFile.Close();
            }
        }
        #endregion

        #region CalculateTheaterCount
        public double CalculateTheaterCount(double val, DateTime from, DateTime to)
        {
            return val * (this._theaterCountDictionary[to.Year] / this._theaterCountDictionary[from.Year]);
        } 
        #endregion
    }
}
