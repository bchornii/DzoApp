using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Parser.Html;
using Application.Web.Models;

namespace Application.Web.Custom
{
    public static class Parser
    {        
        public static List<FragmentInformation> CompanyFragmentToObject(string html)
        {
            var helper = new HtmlParser();
            var doc = helper.Parse(html);
            var elementsList = new List<FragmentInformation>();

            var element =
                doc.All.FirstOrDefault(e => e.ClassList.Contains("list") &&
                                   e.ClassList.Contains("list2") &&
                                   e.ClassList.Contains("list3"));

            var items = element?.Children?.Select(c => c.InnerHtml);

            if (items != null)
            {
                foreach (var item in items)
                {
                    var el = new FragmentInformation();

                    // Parse current item
                    var fragment = helper.Parse(item);

                    // Get item name and link
                    var nameLinkItem = fragment.All.FirstOrDefault(f => f.ClassList.Contains("reverse") &&
                                                                     f.ClassList.Contains("tenderLink"));

                    el.Name = nameLinkItem?.TextContent;
                    el.Link = (nameLinkItem as AngleSharp.Dom.Html.IHtmlAnchorElement)?.PathName;

                    // Get expected price
                    var expectedPriceItem = fragment.All.FirstOrDefault(f => f.ClassList.Contains("l") &&
                                                                             f.ClassList.Contains("name") &&
                                                                             f.ClassList.Contains("kvaziName"));

                    el.ExpectedPrice = expectedPriceItem?.TextContent;

                    // Get expected price
                    var codeItem = fragment.All.FirstOrDefault(f => f.ClassList.Contains("l") &&
                                                                    f.ClassList.Contains("code"));
                    el.Code = codeItem?.TextContent;

                    // Get expected status
                    var statusItem = fragment.All.FirstOrDefault(f => f.ClassList.Contains("l") &&
                                                                      f.ClassList.Contains("status"));
                    el.Status = statusItem?.TextContent;

                    // Get expected auction date
                    var auctionDate = fragment.All.FirstOrDefault(f => f.ClassList.Contains("l") &&
                                                                       f.ClassList.Contains("auction"));
                    el.AuctionDate = auctionDate?.TextContent;

                    // Get organization
                    var organizationItem = fragment.All.FirstOrDefault(f => f.ClassList.Contains("l") &&
                                                                            f.ClassList.Contains("org"));                    
                    el.Organization = organizationItem?.TextContent;

                    // Get code id/ code value/ code desc
                    var cvpCdItem = fragment.All.FirstOrDefault(f => f.ClassList.Contains("cpv") &&
                                                                 f.ClassList.Contains("cd"));
                    el.CodeId = cvpCdItem?.TextContent;                                      
                    el.CodeDesc = string.Empty;
                    el.CodeValue = string.Empty;

                    // Get buy id/ buy type
                    var buyIdItem = fragment.All.FirstOrDefault(f => f.ClassList.Length == 1 &&
                                                                 f.ClassName == "cd");                    
                    el.BuyId = buyIdItem?.TextContent;                    
                    el.BuyType = string.Empty;

                    var buyTypeItem = fragment.All.FirstOrDefault(f => f.ClassList.Contains("cd") &&
                                                                       f.ClassList.Contains("tenderMethod"));                   
                    el.BuyProcedureName = buyTypeItem?.TextContent;                
                    el.BuyProcedureValue = string.Empty;

                    elementsList.Add(el);
                }
            }
            return elementsList;
        }

        public static List<TenderInformation> TenderFragmentsToObjects(string[] tenders)
        {
            var tendersObj = new List<TenderInformation>();
            // Parse result
            foreach (var tender in tenders)
            {                
                var helper = new HtmlParser();
                var element = helper.Parse(tender);
                var tenderobj = new TenderInformation();

                var targetElement = element.All.FirstOrDefault(e => e.ClassList.Contains("tendersForm") &&
                                                            e.ClassList.Contains("relative"));                

                var targetElementText = Regex.Replace(targetElement?.TextContent ?? string.Empty, @"\t|\n|\r", "#")
                                       .Split('#')
                                       .Where(str => str.Length > 2)
                                       .ToList();

                var ownerNameIndex = targetElementText.IndexOf("Найменування замовника");
                var addressIndex = targetElementText.IndexOf("Юридична адреса");
                var contactPersonIndex = targetElementText.IndexOf("Ім'я");
                var phoneIndex = targetElementText.IndexOf("Телефон");
                var emailIndex = targetElementText.IndexOf("E-mail");
                var priceIndex = targetElementText.IndexOf("Очікувана вартість");

                tenderobj.Owner = targetElementText.Count > ownerNameIndex ? targetElementText[ownerNameIndex + 1] : string.Empty;
                tenderobj.Address = targetElementText.Count > addressIndex ? targetElementText[addressIndex + 1] : string.Empty;
                tenderobj.ContactPerson = targetElementText.Count > contactPersonIndex
                    ? targetElementText[contactPersonIndex + 1]
                    : string.Empty;
                tenderobj.Phone = targetElementText.Count > phoneIndex ? targetElementText[phoneIndex + 1] : string.Empty;
                tenderobj.Email = targetElementText.Count > emailIndex ? targetElementText[emailIndex + 1] : string.Empty;
                tenderobj.ProposePrice = targetElementText.Count > priceIndex
                    ? targetElementText[priceIndex + 1]
                    : string.Empty;
                
                tendersObj.Add(tenderobj);                    
            }
            return tendersObj;
        }
    }
}