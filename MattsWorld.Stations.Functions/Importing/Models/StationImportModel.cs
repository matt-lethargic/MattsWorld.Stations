using FileHelpers;

namespace MattsWorld.Stations.Functions.Importing.Models
{
    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    public class StationImportModel
    {
        [FieldFixedLength(1)]
        public string RecordType;

        [FieldFixedLength(34)]
        [FieldTrim(TrimMode.Both)]
        public string StationName;

        [FieldFixedLength(1)]
        public int CateType;

        [FieldFixedLength(7)]
        [FieldTrim(TrimMode.Both)]
        public string TipLocCode;

        [FieldFixedLength(3)]
        [FieldTrim(TrimMode.Both)]
        public string SubsidiaryThreeAlphaCode;

        [FieldFixedLength(6)]
        [FieldTrim(TrimMode.Both)]
        public string ThreeAlphaCode;

        [FieldFixedLength(5)]
        public int Eastings;

        [FieldFixedLength(1)]
        [FieldConverter(ConverterKind.Boolean, "E", "")]
        [FieldNullValue(false)]
        public bool Estimated;

        [FieldFixedLength(5)]
        public int Northings;

        [FieldFixedLength(2)]
        public int ChangeTime;

        [FieldFixedLength(13)]
        [FieldTrim(TrimMode.Both)]
        public string FootNote;

        [FieldFixedLength(3)]
        [FieldTrim(TrimMode.Both)]
        public string Region;
    }
}
