namespace EnrollmentStation.Code.Enums
{
    public enum YubicoNeoModeEnum : byte
    {
        OtpOnly = 0,
        CcidOnly = 1,
        OtpCcid = 2,
        U2fOnly = 3,
        OtpU2f = 4,
        U2fCcid = 5,
        OtpU2fCcid = 6,
        OtpOnly_WithEject = 0 + 0x80,
        CcidOnly_WithEject = 1 + 0x80,
        OtpCcid_WithEject = 2 + 0x80,
        U2fOnly_WithEject = 3 + 0x80,
        OtpU2f_WithEject = 4 + 0x80,
        U2fCcid_WithEject = 5 + 0x80,
        OtpU2fCcid_WithEject = 6 + 0x80
    }
}