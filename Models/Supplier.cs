namespace pricheson.Models

{
    public class Supplier
        
    {

        public List<Timetable> timetable { get; set; }
        public List<DateandTime> dateandtime { get; set; }
        public string autorization { get; set; }

        public Sendingfreetime sendingfreetime { get; set; }
        public List<DateAndTimeForSorting> dateandtimeforsorting { get; set; }
    }
    
}
