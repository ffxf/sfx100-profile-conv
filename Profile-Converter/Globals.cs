namespace SFB_Profile_Converter
{
    public static class Globals
    {
        // To avoid we have to instantiate ConsoleOut many time or pass instances around
        // We use ConsoleOut to be able to toggle between verbose/quiet behavior
        public static ConsoleOut co = new ConsoleOut(false); // 'false' --> default verbose
    }
}