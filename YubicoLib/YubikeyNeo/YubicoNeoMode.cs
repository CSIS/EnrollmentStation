using System;

namespace YubicoLib.YubikeyNeo
{
    public class YubicoNeoMode
    {
        public YubicoNeoModeEnum Mode
        {
            get
            {
                if (HasEjectMode && HasOtp && HasU2f && HasCcid)
                    return YubicoNeoModeEnum.OtpU2fCcid_WithEject;

                if (HasEjectMode && !HasOtp && HasU2f && HasCcid)
                    return YubicoNeoModeEnum.U2fCcid_WithEject;

                if (HasEjectMode && HasOtp && HasU2f && !HasCcid)
                    return YubicoNeoModeEnum.OtpU2f_WithEject;

                if (HasEjectMode && !HasOtp && HasU2f && !HasCcid)
                    return YubicoNeoModeEnum.U2fOnly_WithEject;

                if (HasEjectMode && HasOtp && !HasU2f && HasCcid)
                    return YubicoNeoModeEnum.OtpCcid_WithEject;

                if (HasEjectMode && !HasOtp && !HasU2f && HasCcid)
                    return YubicoNeoModeEnum.CcidOnly_WithEject;

                if (HasEjectMode && HasOtp && !HasU2f && !HasCcid)
                    return YubicoNeoModeEnum.OtpOnly_WithEject;

                if (!HasEjectMode && HasOtp && HasU2f && HasCcid)
                    return YubicoNeoModeEnum.OtpU2fCcid;

                if (!HasEjectMode && !HasOtp && HasU2f && HasCcid)
                    return YubicoNeoModeEnum.U2fCcid;

                if (!HasEjectMode && HasOtp && HasU2f && !HasCcid)
                    return YubicoNeoModeEnum.OtpU2f;

                if (!HasEjectMode && !HasOtp && HasU2f && !HasCcid)
                    return YubicoNeoModeEnum.U2fOnly;

                if (!HasEjectMode && HasOtp && !HasU2f && HasCcid)
                    return YubicoNeoModeEnum.OtpCcid;

                if (!HasEjectMode && !HasOtp && !HasU2f && HasCcid)
                    return YubicoNeoModeEnum.CcidOnly;

                return YubicoNeoModeEnum.OtpOnly;
            }
            set
            {
                HasOtp = HasCcid = HasU2f = HasEjectMode = false;

                switch (value)
                {
                    case YubicoNeoModeEnum.OtpOnly:
                        HasOtp = true;
                        break;
                    case YubicoNeoModeEnum.CcidOnly:
                        HasCcid = true;
                        break;
                    case YubicoNeoModeEnum.OtpCcid:
                        HasOtp = HasCcid = true;
                        break;
                    case YubicoNeoModeEnum.U2fOnly:
                        HasU2f = true;
                        break;
                    case YubicoNeoModeEnum.OtpU2f:
                        HasOtp = HasU2f = true;
                        break;
                    case YubicoNeoModeEnum.U2fCcid:
                        HasU2f = HasCcid = true;
                        break;
                    case YubicoNeoModeEnum.OtpU2fCcid:
                        HasOtp = HasU2f = HasCcid = true;
                        break;
                    case YubicoNeoModeEnum.OtpOnly_WithEject:
                        HasOtp = HasEjectMode = true;
                        break;
                    case YubicoNeoModeEnum.CcidOnly_WithEject:
                        HasCcid = HasEjectMode = true;
                        break;
                    case YubicoNeoModeEnum.OtpCcid_WithEject:
                        HasOtp = HasCcid = HasEjectMode = true;
                        break;
                    case YubicoNeoModeEnum.U2fOnly_WithEject:
                        HasU2f = HasEjectMode = true;
                        break;
                    case YubicoNeoModeEnum.OtpU2f_WithEject:
                        HasOtp = HasU2f = HasEjectMode = true;
                        break;
                    case YubicoNeoModeEnum.U2fCcid_WithEject:
                        HasU2f = HasCcid = HasEjectMode = true;
                        break;
                    case YubicoNeoModeEnum.OtpU2fCcid_WithEject:
                        HasOtp = HasU2f = HasCcid = HasEjectMode = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("value");
                }
            }
        }

        public bool HasOtp { get; set; }
        public bool HasCcid { get; set; }
        public bool HasU2f { get; set; }
        public bool HasEjectMode { get; set; }

        public bool IsValid { get { return HasOtp || HasCcid || HasU2f; } }

        public YubicoNeoMode(YubicoNeoModeEnum mode)
        {
            Mode = mode;
        }

        public override string ToString()
        {
            string res = string.Empty;

            if (HasOtp)
                res += "OTP";

            if (HasCcid && res.Length > 0)
                res += "+CCID";
            else if (HasCcid)
                res += "CCID";

            if (HasU2f && res.Length > 0)
                res += "+U2F";
            else if (HasU2f)
                res += "U2F";

            return res;
        }
    }
}