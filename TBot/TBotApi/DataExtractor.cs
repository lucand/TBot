using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Windows.Forms;
using System.IO;

namespace TBot.TajperApi
{
    public class DataExtractor
    {
        List<Bet> bets;

        public DataExtractor()
        {
            bets = new List<Bet>();
            getBetDataFromHTML();
        }

        string GetFilePath()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.FileName;
                }
            }

            return null;
        }

        void getBetDataFromHTML()
        {

            string betClass = "//div[@class='bet clearfix']"; //todo parametr aplikacji

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(GetFilePath(), Encoding.UTF8);

            List<Bet> betsData = new List<Bet>();

            foreach (HtmlNode betNode in doc.DocumentNode.SelectNodes(betClass))
            {
                Dictionary<string, string> betRawData = new Dictionary<string, string>();
                foreach (HtmlNode itemNode in betNode.ChildNodes)
                {
                    if (itemNode.Attributes.Contains("class") && itemNode.Attributes["class"].Value == "title")
                    {
                        var playersNode = itemNode.SelectSingleNode(".//strong");
                        betRawData.Add("players", playersNode.InnerText);

                        foreach (HtmlNode smallNode in itemNode.SelectNodes(".//small"))
                        {
                            if (smallNode.Attributes.Contains("class") && smallNode.Attributes["class"].Value == "date")
                            {
                                betRawData.Add("date", smallNode.InnerText);
                            }
                            else if (smallNode.InnerText.StartsWith("Tenis"))
                            {
                                betRawData.Add("league", smallNode.InnerText);
                            }
                        }
                    }
                    else if (itemNode.InnerText.StartsWith("Typ"))
                    {
                        betRawData.Add("type", itemNode.InnerText);
                    }
                    else if (itemNode.InnerText.StartsWith("Stawka"))
                    {
                        betRawData.Add("bid", itemNode.InnerText);
                    }
                    else if (itemNode.InnerText.StartsWith("Ocena"))
                    {
                        betRawData.Add("chancesMin", itemNode.InnerText);
                    }
                    else if (itemNode.InnerText.StartsWith("Kurs"))
                    {
                        betRawData.Add("exchange", itemNode.InnerText);
                    }
                    else if (itemNode.InnerText.StartsWith("Przewidywany"))
                    {
                        betRawData.Add("predictedProfit", itemNode.InnerText);
                    }
                }
                
                betsData.Add(new Bet(betRawData));
            }

            betsData.Clear();
        }
    }
}
