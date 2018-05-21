using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

// vCard Reader

namespace MyProject.vCard
{
    /// <summary>
    /// You may combine HomeWorkType and PhoneType, and FLAG them to reflect the attributes of vCard.
    /// </summary>
    public enum HomeWorkType
    {
        home, work
    }

    public enum PhoneType
    {
        VOICE,
        FAX,
        MSG,
        CELL,
        PAGER

    }
    /// <summary>
    /// If you flag the enume types, you may use flags.
    /// </summary>
    public struct Phone
    {
        public string number;
        public HomeWorkType homeWorkType;
        public bool pref;
        public PhoneType phoneType;

    }

    public struct Email
    {
        public string address;
        public HomeWorkType homeWorkType;
        public bool pref;
    }

    public struct Address
    {
        public string po;
        public string ext;
        public string street;
        public string locality;
        public string region;
        public string postcode;
        public string country;

        public HomeWorkType homeWorkType;
    }
    public enum LabelType
    {
        DOM,
        INTL,
        POSTAL,
        PARCEL

    }

    /// <summary>
    /// Not used yet. You may use regular expressions or String.Replace() to replace =0D=0A to line breaks.
    /// </summary>
    public struct Label
    {
        public string address;
        public LabelType labelType;
    }

    /// <summary>
    /// Read text and create data fields of collections.
    /// </summary>
    public class vCardReader
    {

        #region Singlar Properties

        private string formattedName;

        public string FormattedName
        {
            get { return formattedName; }
            set { formattedName = value; }
        }

        string surname;
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        private string givenName;

        public string GivenName
        {
            get { return givenName; }
            set { givenName = value; }
        }
        private string middleName;

        public string MiddleName
        {
            get { return middleName; }
            set { middleName = value; }
        }

        private string prefix;

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }
        private string suffix;

        public string Suffix
        {
            get { return suffix; }
            set { suffix = value; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private DateTime? bday;

        public DateTime? Birthday
        {
            get { return bday; }
            set { bday = value; }
        }

        private DateTime? rev;
        /// <summary>
        /// If Rev in vCard is UTC, Rev will convert utc to local datetime.
        /// </summary>
        public DateTime? Rev
        {
            get { return rev; }
            set { rev = value; }
        }

        private string org;

        public string Org
        {
            get { return org; }
            set { org = value; }
        }

        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }



        #endregion


        #region Property Collections with attribute

        private List<Address> addresses = new List<Address>();

        public List<Address> Addresses
        {
            get { return addresses; }
            //set { addresses = value; }
        }

        private List<Phone> phones = new List<Phone>();

        public List<Phone> Phones
        {
            get { return phones; }
            //set { phones = value; }
        }

        private List<Email> emails = new List<Email>();

        public List<Email> Emails
        {
            get { return emails; }
            //set { emails = value; }
        }

        private List<string> urls = new List<string>();
        public List<string> URLs
        {
            get { return urls; }
        }

        #endregion

        /// <summary>
        /// Analyze s into vCard structures.
        /// </summary>
        public void ParseLines(string s)
        {
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace;

            Regex regex;
            Match m;
            MatchCollection mc;

            regex = new Regex(@"(?<strElement>(FN))   (:(?<strFN>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
                FormattedName = m.Groups["strFN"].Value;

            regex = new Regex(@"(\n(?<strElement>(N)))   (:(?<strSurname>([^;]*))) (;(?<strGivenName>([^;]*)))  (;(?<strMidName>([^;]*))) (;(?<strPrefix>([^;]*))) (;(?<strSuffix>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
            {
                Surname = m.Groups["strSurname"].Value;
                GivenName = m.Groups["strGivenName"].Value;
                MiddleName = m.Groups["strMidName"].Value;
                Prefix = m.Groups["strPrefix"].Value;
                Suffix = m.Groups["strSuffix"].Value;
            }

            ///Title
            regex = new Regex(@"(?<strElement>(TITLE))   (:(?<strTITLE>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
                Title = m.Groups["strTITLE"].Value;

            ///ORG
            regex = new Regex(@"(?<strElement>(ORG))   (:(?<strORG>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
                Org = m.Groups["strORG"].Value;

            ///Note
            regex = new Regex(@"((?<strElement>(NOTE)) (;*(?<strAttr>(ENCODING=QUOTED-PRINTABLE)))*  ([^:]*)*  (:(?<strValue> (([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) )))", options);
            m = regex.Match(s);
            if (m.Success)
            {
                Note = m.Groups["strValue"].Value;
                //Remove connections and escape strings. The order is significant.
                Note = Note.Replace("=" + Environment.NewLine, "");
                Note = Note.Replace("=0D=0A" , Environment.NewLine);
                Note = Note.Replace("=3D", "=");
            }

            ///Birthday
            regex = new Regex(@"(?<strElement>(BDAY)) [^:]*  (:(?<strBDAY>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
            {
                string[] expectedFormats = { "yyyyMMdd", "yyMMdd", "yyyy-MM-dd" };
                Birthday = DateTime.ParseExact(m.Groups["strBDAY"].Value, expectedFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            }

            ///Rev
            regex = new Regex(@"(?<strElement>(REV))   (:(?<strREV>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
            {
                string[] expectedFormats = { "yyyyMMddHHmmss", "yyyyMMddTHHmmssZ" };
                Rev = DateTime.ParseExact(m.Groups["strREV"].Value, expectedFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            }

            ///Emails
            string ss;

            regex = new Regex(@"(\.(?<strElement>(EMAIL)) (;*(?<strAttr>(HOME|WORK)))*  (;(?<strPref>(PREF)))* (;[^:]*)*  (:(?<strValue>[^\n\r]*)))", options);
            mc = regex.Matches(s);
            if (mc.Count > 0)
            {
                //Emails = new Email[mc.Count];
                for (int i = 0; i < mc.Count; i++)
                {
                    m = mc[i];
                    Email email = new Email();
                    email.address = m.Groups["strValue"].Value;
                    ss = m.Groups["strAttr"].Value;
                    if (ss == "HOME")
                        email.homeWorkType = HomeWorkType.home;
                    else if (ss == "WORK")
                        email.homeWorkType = HomeWorkType.work;

                    if (m.Groups["strPref"].Value == "PREF")
                        email.pref = true;

                    emails.Add(email);
                }
            }

            regex = new Regex(@"(\.(?<strElement>(URL))  (;type=(?<strPref>(PREF)))* (;[^:]*)*  (:(?<strValue>[^\n\r]*)))", options);
            mc = regex.Matches(s);
            if (mc.Count > 0)
            {
                //Emails = new Email[mc.Count];
                for (int i = 0; i < mc.Count; i++)
                {
                    m = mc[i];
                    urls.Add(m.Groups["strValue"].Value);
                    /*m = mc[i];
                    Email email = new Email();
                    email.address = m.Groups["strValue"].Value;
                    ss = m.Groups["strAttr"].Value;
                    if (ss == "HOME")
                        email.homeWorkType = HomeWorkType.home;
                    else if (ss == "WORK")
                        email.homeWorkType = HomeWorkType.work;

                    if (m.Groups["strPref"].Value == "PREF")
                        email.pref = true;

                    emails.Add(email);*/
                }
            }

            ///Phones
            regex = new Regex(@"(\.(?<strElement>(TEL)) (;*(?<strAttr>(HOME|WORK)))* (;(?<strType>(VOICE|CELL|PAGER|MSG|FAX)))*  (;(?<strPref>(PREF)))* (;[^:]*)*  (:(?<strValue>[^\n\r]*)))", options);
            mc = regex.Matches(s);
            if (mc.Count > 0)
            {
                //Phones = new Phone[mc.Count];
                for (int i = 0; i < mc.Count; i++)
                {
                    m = mc[i];
                    Phone phone = new Phone();

                    phone.number = m.Groups["strValue"].Value;
                    ss = m.Groups["strAttr"].Value;
                    if (ss == "HOME")
                        phone.homeWorkType = HomeWorkType.home;
                    else if (ss == "WORK")
                        phone.homeWorkType = HomeWorkType.work;

                    if (m.Groups["strPref"].Value == "PREF")
                        phone.pref = true;

                    ss = m.Groups["strType"].Value;
                    if (ss == "VOICE")
                        phone.phoneType = PhoneType.VOICE;
                    else if (ss == "CELL")
                        phone.phoneType = PhoneType.CELL;
                    else if (ss == "PAGER")
                        phone.phoneType = PhoneType.PAGER;
                    else if (ss == "MSG")
                        phone.phoneType = PhoneType.MSG;
                    else if (ss == "FAX")
                        phone.phoneType = PhoneType.FAX;

                    phones.Add(phone);
                }
            }
            ///Addresses
            regex = new Regex(@"(\.(?<strElement>(ADR))) (;type=*(?<strAttr>(HOME|WORK)))*  (;type=pref)* (.(?<strPo>([^;]*)))  (;(?<strBlock>([^;]*)))  (;(?<strStreet>([^;]*)))  (;(?<strCity>([^;]*))) (;(?<strRegion>([^;]*))) (;(?<strPostcode>([^;]*)))(;(?<strNation>[^\n\r]*)) ", options);
            mc = regex.Matches(s);
            if (mc.Count > 0)
            {
                //Addresses = new Address[mc.Count];
                for (int i = 0; i < mc.Count; i++)
                {
                    m = mc[i];
                    Address address = new Address();
                    ss = m.Groups["strAttr"].Value;
                    if (ss == "HOME")
                        address.homeWorkType = HomeWorkType.home;
                    else if (ss == "WORK")
                        address.homeWorkType = HomeWorkType.work;

                    address.po = m.Groups["strPo"].Value;
                    address.ext = m.Groups["strBlock"].Value;
                    address.street = m.Groups["strStreet"].Value;
                    address.locality = m.Groups["strCity"].Value;
                    address.region = m.Groups["strRegion"].Value;
                    address.postcode = m.Groups["strPostcode"].Value;
                    address.country = m.Groups["strNation"].Value;

                    addresses.Add(address);
                }
            }


        }



    }
}
