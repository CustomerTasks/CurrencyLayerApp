using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using NUnit.Framework;

namespace CurrencyLayerApp.Tests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void CheckParsing()
        {
            //var path = Environment.CurrentDirectory;
            var res = Parsers.ParseCurrencyModels(@"D:\Clouds\MegaNZ\Freelance\UpWork\Moshe Do- CurrencyLayer\CurrencyLayerApp\CurrencyLayerApp\Resources\Currencies.txt");
            Assert.True(res.Any());
        }
    }
}
