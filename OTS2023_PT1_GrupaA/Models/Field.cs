

namespace OTS2026_PT1_GrupaB.Models
{

    public enum Zone
    {
        Valid,
        Invalid
    }

    public enum FieldContent
    {
        Empty,
        Clover,
        Sheep,
        SheepDog
    }

    public class Field
    {
        public FieldContent Content { get; set; }
        public Zone Zone { get; set; }

        public Field(Zone zone)
        {
            Zone = zone;
            Content = FieldContent.Empty;
        }

        public Field(Zone zone, FieldContent content)
        {
            Zone = zone;
            Content = content;
        }
    }        
}
