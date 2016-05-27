using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Windows.Forms;
using System.IO;
using TBot.Helpers;
using TBot.Properties;

namespace TBot.DataExtraction
{
    public class DataExtractor
    {
        List<Bet> bets;

        public DataExtractor()
        {
            bets = new List<Bet>();
            getBetsDataFromHTML();
        }

        string GetFilePath()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = Resources.htmlExtension + "|" + Resources.allFilesExtension;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.FileName;
                }
            }

            return null;
        }

        void getBetsDataFromHTML()
        {
            try
            {
                string filePath = GetFilePath();
                if (String.IsNullOrEmpty(filePath)) return;

                string betClass = ConfigHelper.webTemplate["betClass"];

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.Load(filePath, Encoding.UTF8);

                List<Bet> betsData = new List<Bet>();

                foreach (HtmlNode betNode in doc.DocumentNode.SelectNodes(betClass))
                {
                    Dictionary<string, string> betRawData = new Dictionary<string, string>();
                    foreach (HtmlNode itemNode in betNode.ChildNodes)
                    {
                        if (itemNode.Attributes.Contains("class") && itemNode.Attributes["class"].Value == "title")
                        {
                            var playersNode = itemNode.SelectSingleNode(".//strong");
                            betRawData.Add("players", playersNode.InnerText.Trim());

                            foreach (HtmlNode smallNode in itemNode.SelectNodes(".//small"))
                            {
                                if (smallNode.Attributes.Contains("class") && smallNode.Attributes["class"].Value == "date")
                                {
                                    betRawData.Add("date", smallNode.InnerText.Trim());
                                }
                                else if (smallNode.InnerText.StartsWith(ConfigHelper.webTemplate["league"]))
                                {
                                    betRawData.Add("league", smallNode.InnerText.Trim());
                                }
                            }
                        }
                        else if (itemNode.InnerText.StartsWith("Typ"))
                        {
                            betRawData.Add("type", itemNode.InnerText.Trim());
                        }
                        else if (itemNode.InnerText.StartsWith(ConfigHelper.webTemplate["bid"]))
                        {
                            betRawData.Add("bid", itemNode.InnerText.Trim());
                        }
                        else if (itemNode.InnerText.StartsWith(ConfigHelper.webTemplate["chancesMin"]))
                        {
                            betRawData.Add("chancesMin", itemNode.InnerText.Trim());
                        }
                        else if (itemNode.InnerText.StartsWith(ConfigHelper.webTemplate["exchange"]))
                        {
                            betRawData.Add("exchange", itemNode.InnerText.Trim());
                        }
                        else if (itemNode.InnerText.StartsWith(ConfigHelper.webTemplate["predictedProfit"]))
                        {
                            betRawData.Add("predictedProfit", itemNode.InnerText.Trim());
                        }
                    }

                    betsData.Add(new Bet(betRawData));
                }

                
            }
            catch (Exception exc)
            {
                MessageBox.Show("getBetDataFromHTML -> " + exc.Message, Resources.error);
            }
        }
    }
}
