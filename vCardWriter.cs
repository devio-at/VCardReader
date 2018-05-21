using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.vCard
{
    /*class vCardWriter : vCardReader
    {
        public vCardWriter() :base()
        {

        }

        public string Text
        {
            get
            {
                string r="";
                r += "BEGIN:VCARD" + Environment.NewLine + "VERSION:2.1" + Environment.NewLine;
                r += "N:" + Surname + ";" + GivenName + ";" + MiddleName + ";" + Prefix + ";" + Suffix + Environment.NewLine;
                r += "FN:" + FormattedName + Environment.NewLine;
                if (!String.IsNullOrEmpty(Title))
                    r += "TITLE:" + Title + Environment.NewLine;
                if (!String.IsNullOrEmpty(Org))
                    r += "ORG:" + Title + Environment.NewLine;

                if (Birthday != null)
                    r += "BDAY:" + Birthday.ToString("yyyyMMdd") + Environment.NewLine;

                foreach (Email email in Emails)
                {
                    r += "EMAIL" + (email.pref ? ";PREF" : "") + ( (email.homeWorkType == HomeWorkType.home) ? ";HOME" : ";WORK") + ":" + email.address + Environment.NewLine;
                }

                foreach (Phone p in Phones)
                {
                    r += "TEL" + (p.pref ? ";PREF" : "") + ((p.homeWorkType == HomeWorkType.home) ? ";HOME" : ";WORK") + ";"+ p.phoneType.ToString() +":" + p.number + Environment.NewLine;
                }

                foreach (Address adr in Addresses)
                {
                    r += "ADR:" + ((adr.homeWorkType == HomeWorkType.home) ? ";HOME" : ";WORK") + ":" + adr.po + ";" + adr.ext +
                        ";" + adr.street + ";" + adr.region + ";" + adr.postcode + ";" + adr.country + Environment.NewLine;
                }

                // do notes
                // do revison

                return r;

            }
        }
    }*/
}
