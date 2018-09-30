using System;

namespace ReveilDriver
{

    public class UtilReveilDriver
    {

        // Mise sur deux caractères de l'information
        private String twochar(string c)
        {

            String r = "00";

            if (c.Length < 2)
            {
                r = "0" + c;
            }
            else
            {
                r = c;
            }

            return r;
        }

        // Construction de la date
        public String date()
        {

            DateTime _DateTime = DateTime.Now;

            String _DayOfWeekComplete = _DateTime.DayOfWeek.ToString();

            String _DayOfWeekFr = "lun";

            if (_DayOfWeekComplete.Equals("Monday"))
            {

                _DayOfWeekFr = "lun";
            }
            if (_DayOfWeekComplete.Equals("Tuesday"))
            {

                _DayOfWeekFr = "mar";
            }
            if (_DayOfWeekComplete.Equals("Wednesday"))
            {

                _DayOfWeekFr = "mer";
            }
            if (_DayOfWeekComplete.Equals("Thursday"))
            {

                _DayOfWeekFr = "jeu";
            }
            if (_DayOfWeekComplete.Equals("Friday"))
            {

                _DayOfWeekFr = "ven";
            }
            if (_DayOfWeekComplete.Equals("Saturday"))
            {

                _DayOfWeekFr = "sam";
            }
            if (_DayOfWeekComplete.Equals("Sunday"))
            {

                _DayOfWeekFr = "dim";
            }

            String _Day = _DateTime.Day.ToString();
            String _Month = _DateTime.Month.ToString();
            String _Year = _DateTime.Year.ToString();

            return _DayOfWeekFr + " " + twochar(_Day) + "/" + twochar(_Month) + "/" + _Year;
        }

        // Construction de l'heure
        public String heure()
        {

            DateTime dateTime = DateTime.Now;
            return twochar(dateTime.Hour.ToString());
        }

        // Construction des minutes
        public String minute()
        {

            DateTime dateTime = DateTime.Now;
            return twochar(dateTime.Minute.ToString());
        }

        // Construction des secondes
        public String seconde()
        {

            DateTime dateTime = DateTime.Now;
            return twochar(dateTime.Second.ToString());
        }

        // Construction des milisecondes
        public String milliSeconde()
        {

            DateTime dateTime = DateTime.Now;
            return twochar(dateTime.Millisecond.ToString());
        }

        // Ajout d'une heure
        public String addHour(String hour)
        {

            String _hour = "06";

            if (hour.Equals("00"))
            {
                _hour = "01";
            }
            else if (hour.Equals("01"))
            {
                _hour = "02";
            }
            else if (hour.Equals("02"))
            {
                _hour = "03";
            }
            else if (hour.Equals("03"))
            {
                _hour = "04";
            }
            else if (hour.Equals("04"))
            {
                _hour = "05";
            }
            else if (hour.Equals("05"))
            {
                _hour = "06";
            }
            else if (hour.Equals("06"))
            {
                _hour = "07";
            }
            else if (hour.Equals("07"))
            {
                _hour = "08";
            }
            else if (hour.Equals("08"))
            {
                _hour = "09";
            }
            else if (hour.Equals("09"))
            {
                _hour = "10";
            }
            else if (hour.Equals("10"))
            {
                _hour = "11";
            }
            else if (hour.Equals("11"))
            {
                _hour = "12";
            }
            else if (hour.Equals("12"))
            {
                _hour = "13";
            }
            else if (hour.Equals("13"))
            {
                _hour = "14";
            }
            else if (hour.Equals("14"))
            {
                _hour = "15";
            }
            else if (hour.Equals("15"))
            {
                _hour = "16";
            }
            else if (hour.Equals("16"))
            {
                _hour = "17";
            }
            else if (hour.Equals("17"))
            {
                _hour = "18";
            }
            else if (hour.Equals("18"))
            {
                _hour = "19";
            }
            else if (hour.Equals("19"))
            {
                _hour = "20";
            }
            else if (hour.Equals("20"))
            {
                _hour = "21";
            }
            else if (hour.Equals("21"))
            {
                _hour = "22";
            }
            else if (hour.Equals("22"))
            {
                _hour = "23";
            }
            else if (hour.Equals("23"))
            {
                _hour = "00";
            }

            return _hour;

        }

        // Retrait d'une heure
        public String removeHour(String hour)
        {

            String _hour = "06";

            if (hour.Equals("00"))
            {
                _hour = "23";
            }
            else if (hour.Equals("23"))
            {
                _hour = "22";
            }
            else if (hour.Equals("22"))
            {
                _hour = "21";
            }
            else if (hour.Equals("21"))
            {
                _hour = "20";
            }
            else if (hour.Equals("20"))
            {
                _hour = "19";
            }
            else if (hour.Equals("19"))
            {
                _hour = "18";
            }
            else if (hour.Equals("18"))
            {
                _hour = "17";
            }
            else if (hour.Equals("17"))
            {
                _hour = "16";
            }
            else if (hour.Equals("16"))
            {
                _hour = "15";
            }
            else if (hour.Equals("15"))
            {
                _hour = "14";
            }
            else if (hour.Equals("14"))
            {
                _hour = "13";
            }
            else if (hour.Equals("13"))
            {
                _hour = "12";
            }
            else if (hour.Equals("12"))
            {
                _hour = "11";
            }
            else if (hour.Equals("11"))
            {
                _hour = "10";
            }
            else if (hour.Equals("10"))
            {
                _hour = "09";
            }
            else if (hour.Equals("09"))
            {
                _hour = "08";
            }
            else if (hour.Equals("08"))
            {
                _hour = "07";
            }
            else if (hour.Equals("07"))
            {
                _hour = "06";
            }
            else if (hour.Equals("06"))
            {
                _hour = "05";
            }
            else if (hour.Equals("05"))
            {
                _hour = "04";
            }
            else if (hour.Equals("04"))
            {
                _hour = "03";
            }
            else if (hour.Equals("03"))
            {
                _hour = "02";
            }
            else if (hour.Equals("02"))
            {
                _hour = "01";
            }
            else if (hour.Equals("01"))
            {
                _hour = "00";
            }

            return _hour;

        }

        // Ajout d'une minute
        public String addMinute(String minute)
        {

            String _minute = "30";

            if (minute.Equals("00"))
            {
                _minute = "01";
            }
            else if (minute.Equals("01"))
            {
                _minute = "02";
            }
            else if (minute.Equals("02"))
            {
                _minute = "03";
            }
            else if (minute.Equals("03"))
            {
                _minute = "04";
            }
            else if (minute.Equals("04"))
            {
                _minute = "05";
            }
            else if (minute.Equals("05"))
            {
                _minute = "06";
            }
            else if (minute.Equals("06"))
            {
                _minute = "07";
            }
            else if (minute.Equals("07"))
            {
                _minute = "08";
            }
            else if (minute.Equals("08"))
            {
                _minute = "09";
            }
            else if (minute.Equals("09"))
            {
                _minute = "10";
            }
            else if (minute.Equals("10"))
            {
                _minute = "11";
            }
            else if (minute.Equals("11"))
            {
                _minute = "12";
            }
            else if (minute.Equals("12"))
            {
                _minute = "13";
            }
            else if (minute.Equals("13"))
            {
                _minute = "14";
            }
            else if (minute.Equals("14"))
            {
                _minute = "15";
            }
            else if (minute.Equals("15"))
            {
                _minute = "16";
            }
            else if (minute.Equals("16"))
            {
                _minute = "17";
            }
            else if (minute.Equals("17"))
            {
                _minute = "18";
            }
            else if (minute.Equals("18"))
            {
                _minute = "19";
            }
            else if (minute.Equals("19"))
            {
                _minute = "20";
            }
            else if (minute.Equals("20"))
            {
                _minute = "21";
            }
            else if (minute.Equals("21"))
            {
                _minute = "22";
            }
            else if (minute.Equals("22"))
            {
                _minute = "23";
            }
            else if (minute.Equals("23"))
            {
                _minute = "24";
            }
            else if (minute.Equals("24"))
            {
                _minute = "25";
            }
            else if (minute.Equals("25"))
            {
                _minute = "26";
            }
            else if (minute.Equals("26"))
            {
                _minute = "27";
            }
            else if (minute.Equals("27"))
            {
                _minute = "28";
            }
            else if (minute.Equals("28"))
            {
                _minute = "29";
            }
            else if (minute.Equals("29"))
            {
                _minute = "30";
            }
            else if (minute.Equals("30"))
            {
                _minute = "31";
            }
            else if (minute.Equals("31"))
            {
                _minute = "32";
            }
            else if (minute.Equals("32"))
            {
                _minute = "33";
            }
            else if (minute.Equals("33"))
            {
                _minute = "34";
            }
            else if (minute.Equals("34"))
            {
                _minute = "35";
            }
            else if (minute.Equals("35"))
            {
                _minute = "36";
            }
            else if (minute.Equals("36"))
            {
                _minute = "37";
            }
            else if (minute.Equals("37"))
            {
                _minute = "38";
            }
            else if (minute.Equals("38"))
            {
                _minute = "39";
            }
            else if (minute.Equals("39"))
            {
                _minute = "40";
            }
            else if (minute.Equals("40"))
            {
                _minute = "41";
            }
            else if (minute.Equals("41"))
            {
                _minute = "42";
            }
            else if (minute.Equals("42"))
            {
                _minute = "43";
            }
            else if (minute.Equals("43"))
            {
                _minute = "44";
            }
            else if (minute.Equals("44"))
            {
                _minute = "45";
            }
            else if (minute.Equals("45"))
            {
                _minute = "46";
            }
            else if (minute.Equals("46"))
            {
                _minute = "47";
            }
            else if (minute.Equals("47"))
            {
                _minute = "48";
            }
            else if (minute.Equals("48"))
            {
                _minute = "49";
            }
            else if (minute.Equals("49"))
            {
                _minute = "50";
            }
            else if (minute.Equals("50"))
            {
                _minute = "51";
            }
            else if (minute.Equals("51"))
            {
                _minute = "52";
            }
            else if (minute.Equals("52"))
            {
                _minute = "53";
            }
            else if (minute.Equals("53"))
            {
                _minute = "54";
            }
            else if (minute.Equals("54"))
            {
                _minute = "55";
            }
            else if (minute.Equals("55"))
            {
                _minute = "56";
            }
            else if (minute.Equals("56"))
            {
                _minute = "57";
            }
            else if (minute.Equals("57"))
            {
                _minute = "58";
            }
            else if (minute.Equals("58"))
            {
                _minute = "59";
            }
            else if (minute.Equals("59"))
            {
                _minute = "00";
            }

            return _minute;

        }

        // Retrait d'une minute
        public String removeMinute(String minute)
        {

            String _minute = "30";

            if (minute.Equals("00"))
            {
                _minute = "59";
            }
            else if (minute.Equals("59"))
            {
                _minute = "58";
            }
            else if (minute.Equals("58"))
            {
                _minute = "57";
            }
            else if (minute.Equals("57"))
            {
                _minute = "56";
            }
            else if (minute.Equals("56"))
            {
                _minute = "55";
            }
            else if (minute.Equals("55"))
            {
                _minute = "54";
            }
            else if (minute.Equals("54"))
            {
                _minute = "53";
            }
            else if (minute.Equals("53"))
            {
                _minute = "52";
            }
            else if (minute.Equals("52"))
            {
                _minute = "51";
            }
            else if (minute.Equals("51"))
            {
                _minute = "50";
            }
            else if (minute.Equals("50"))
            {
                _minute = "49";
            }
            else if (minute.Equals("49"))
            {
                _minute = "48";
            }
            else if (minute.Equals("48"))
            {
                _minute = "47";
            }
            else if (minute.Equals("47"))
            {
                _minute = "46";
            }
            else if (minute.Equals("46"))
            {
                _minute = "45";
            }
            else if (minute.Equals("45"))
            {
                _minute = "44";
            }
            else if (minute.Equals("44"))
            {
                _minute = "43";
            }
            else if (minute.Equals("43"))
            {
                _minute = "42";
            }
            else if (minute.Equals("42"))
            {
                _minute = "41";
            }
            else if (minute.Equals("41"))
            {
                _minute = "40";
            }
            else if (minute.Equals("40"))
            {
                _minute = "39";
            }
            else if (minute.Equals("39"))
            {
                _minute = "38";
            }
            else if (minute.Equals("38"))
            {
                _minute = "37";
            }
            else if (minute.Equals("37"))
            {
                _minute = "36";
            }
            else if (minute.Equals("36"))
            {
                _minute = "35";
            }
            else if (minute.Equals("35"))
            {
                _minute = "34";
            }
            else if (minute.Equals("34"))
            {
                _minute = "33";
            }
            else if (minute.Equals("33"))
            {
                _minute = "32";
            }
            else if (minute.Equals("32"))
            {
                _minute = "31";
            }
            else if (minute.Equals("31"))
            {
                _minute = "30";
            }
            else if (minute.Equals("30"))
            {
                _minute = "29";
            }
            else if (minute.Equals("29"))
            {
                _minute = "28";
            }
            else if (minute.Equals("28"))
            {
                _minute = "27";
            }
            else if (minute.Equals("27"))
            {
                _minute = "26";
            }
            else if (minute.Equals("26"))
            {
                _minute = "25";
            }
            else if (minute.Equals("25"))
            {
                _minute = "24";
            }
            else if (minute.Equals("24"))
            {
                _minute = "23";
            }
            else if (minute.Equals("23"))
            {
                _minute = "22";
            }
            else if (minute.Equals("22"))
            {
                _minute = "21";
            }
            else if (minute.Equals("21"))
            {
                _minute = "20";
            }
            else if (minute.Equals("20"))
            {
                _minute = "19";
            }
            else if (minute.Equals("19"))
            {
                _minute = "18";
            }
            else if (minute.Equals("18"))
            {
                _minute = "17";
            }
            else if (minute.Equals("17"))
            {
                _minute = "16";
            }
            else if (minute.Equals("16"))
            {
                _minute = "15";
            }
            else if (minute.Equals("15"))
            {
                _minute = "14";
            }
            else if (minute.Equals("14"))
            {
                _minute = "13";
            }
            else if (minute.Equals("13"))
            {
                _minute = "12";
            }
            else if (minute.Equals("12"))
            {
                _minute = "11";
            }
            else if (minute.Equals("11"))
            {
                _minute = "10";
            }
            else if (minute.Equals("10"))
            {
                _minute = "09";
            }
            else if (minute.Equals("09"))
            {
                _minute = "08";
            }
            else if (minute.Equals("08"))
            {
                _minute = "07";
            }
            else if (minute.Equals("07"))
            {
                _minute = "06";
            }
            else if (minute.Equals("06"))
            {
                _minute = "05";
            }
            else if (minute.Equals("05"))
            {
                _minute = "04";
            }
            else if (minute.Equals("04"))
            {
                _minute = "03";
            }
            else if (minute.Equals("03"))
            {
                _minute = "02";
            }
            else if (minute.Equals("02"))
            {
                _minute = "01";
            }
            else if (minute.Equals("01"))
            {
                _minute = "00";
            }

            return _minute;

        }

    }

}
