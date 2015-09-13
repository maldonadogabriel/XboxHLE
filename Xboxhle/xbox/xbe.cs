// © Copyright 2015  Gabriel Maldonado.  All Rights Reserved.
// Linkedin: https://www.linkedin.com/pub/gabriel-maldonado/63/457/866

using System;
using System.Text;
using System.Windows.Forms;
namespace xboxhle
{
    public class xbe
    {

        public static System.IO.FileStream pXbe;

        public static byte[] mem = new byte[67108864]; //64mb's
        static int offset;
        //XBE Header Declarations       
        const int _magic_num = 0x48454258;
        static string magic_num;
        static string magic_num_str;
        static string auth_sig;
        public static string base_addr;
        static string size_header;
        static string size_image;
        static string size_image_header;
        static string time_date;
        static string cert_addr;
        static string num_sections;
        static string sect_addr;
        static string init_flags;
        public static string entry_points;
        const string XOR_ENTRY_DEBUG ="0x94859D4B";
        const string XOR_ENTRY_RETAIL ="0xA8FC57AB";
        static string tls_addr;
        static string pe_stack_commit;
        static string pe_heap_reserv;
        static string pe_heap_commit;
        static string pe_base_addr;
        static string pe_size_image;
        static string pe_checksum;
        static string pe_timedata;
        static string debug_pathname_addr;
        static string final_debug_pathname_addr;
        static string debug_filename_addr;
        static string final_debug_filename_addr;
        static string unicode_filename_addr;
        static string final_unicode_filename_addr;
        static string kernel_thunk_addr;
        const string XOR_KERNEL_DEBUG ="0xEFB1F152";
        const string XOR_KERNEL_RETAIL ="0x5B6D40B6";
        static string non_kernel_dir_addr;
        static string num_lib_versions;
        static string lib_vers_addr;
        static string kernel_lib_vers_addr;
        static string xapi_lib_vers_addr;
        static string logo_bitmap_addr;
        static string logo_bitmap_size;

        //XBE Certificate Declarations.
        static string size_cert;
        static string cert_timedate;
        static string cert_title_id;
        public static string cert_title_name;
        static string cert_alt_title_id;
        static string cert_allowed_media;
        static string cert_game_region;
        static string cert_game_rating;
        static string cert_disk_num;
        static string cert_version;
        static string cert_lan_key;
        static string cert_sig_key;
        static string cert_alt_sig_keys;

        //XBE Section Declarations     
        public static int[] sect_flags = new int[0x0100];
        public static string[] sect_virtual_addr = new string[0x0100];
        public static string[] sect_virtual_size = new string[0x0100];
        public static string[] sect_raw_addr = new string[0x0100];
        public static string[] sect_raw_size = new string[0x0100];
        public static string[] sect_name_addr = new string[0x0100];
        public static string[] sect_name_str = new string[0x0100];
        public static string[] sect_name_ref_count = new string[0x0100];
        public static string[] sect_head_shared_ref_count_addr = new string[0x0100];
        public static string[] sect_tail_shared_ref_count_addr = new string[0x0100];
        public static string[] sect_digest = new string[0x0100];

        //XBE Thread Local Storage Declarations
        static string tls_data_start_addr;
        static string tls_data_end_addr;
        static string tls_index_addr;
        static string tls_callback_addr;
        static string tls_size_zero_fill;
        static string tls_chars;

        //XBE Library Version Declarations
        static string lib_name;
        static string lib_major_version;
        static string lib_minor_version;
        static string lib_build_version;
        static string lib_flags;

        
        public static int SubtractOffset(string Addr, string Base)
        {
            int Result = 0;

            if (!(Convert.ToInt32(Addr, 16) == 0)) Result = Convert.ToInt32(Addr, 16) - Convert.ToInt32(Base, 16);
            return Result;     
        }
        public static  int XorOffset(string Addr, string Base)
        {
            int Result = 0;

            if (!(Convert.ToInt32(Addr, 16) == 0)) Result = Convert.ToInt32(Addr, 16) ^ Convert.ToInt32(Base, 16);
            return Result;
        }

        public static void FetchAllowedMedia(uint t_offset)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;

            switch (t_offset)
            {
                case 0x00000000:
                    t.AppendText(" (UNDEFINED)");
                    break;
                case 0x00000001:
                    {
                        t.AppendText(" (MEDIA_TYPE_HARD_DISK)");
                    break;
                    }
                case 0x00000002:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000003:
                    {
                        t.AppendText(" (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000004:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000005:
                    {
                        t.AppendText(" (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000006:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000007:
                    {
                        t.AppendText(" (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000008:
                    {
                        t.AppendText(" (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000009:
                    {
                        t.AppendText(" (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000010:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000011:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000012:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000013:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000014:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000015:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000016:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000017:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000018:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000019:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000020:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x00000021:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000022:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000023:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000024:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000025:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000026:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000027:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000028:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000029:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000030:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000031:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000032:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000033:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000034:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000035:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000036:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000037:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000038:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000039:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000040:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x00000041:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000042:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000043:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000044:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000045:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000046:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000047:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000048:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000049:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000050:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000051:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000052:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000053:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000054:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000055:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000056:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000057:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000058:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000059:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000060:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x00000061:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000062:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000063:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000064:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000065:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000066:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000067:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000068:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000069:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000070:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000071:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000072:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000073:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000074:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000075:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000076:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000077:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000078:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000079:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000080:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x00000081:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000082:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000083:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000084:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000085:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000086:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000087:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000088:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000089:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000090:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000091:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000092:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000093:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000094:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000095:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000096:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000097:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000098:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000099:
                    {
                        t.AppendText(" (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000100:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)");
                        break;
                    }
                case 0x00000101:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000102:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000103:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000104:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000105:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000106:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000107:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000108:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000109:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000110:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000111:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000112:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000113:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000114:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000115:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000116:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000117:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000118:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000119:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000120:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x00000121:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000122:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000123:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000124:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000125:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000126:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000127:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000128:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000129:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000130:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000131:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000132:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000133:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000134:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000135:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000136:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000137:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000138:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000139:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000140:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x00000141:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000142:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000143:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000144:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000145:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000146:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000147:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000148:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000149:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000150:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000151:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000152:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000153:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000154:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000155:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000156:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000157:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000158:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000159:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000160:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x00000161:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000162:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000163:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000164:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000165:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000166:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000167:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000168:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000169:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000170:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000171:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000172:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000173:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000174:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000175:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000176:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000177:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000178:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000179:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000180:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x00000181:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000182:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000183:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000184:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000185:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000186:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000187:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000188:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000189:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000190:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000191:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000192:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000193:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000194:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000195:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000196:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000197:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000198:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000199:
                    {
                        t.AppendText(" (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000200:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)");
                        break;
                    }


                case 0x00000201:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000202:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000203:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000204:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000205:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000206:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000207:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000208:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000209:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000210:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000211:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000212:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000213:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000214:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000215:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000216:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000217:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000218:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000219:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000220:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x00000221:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000222:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000223:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000224:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000225:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000226:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000227:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000228:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000229:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000230:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000231:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000232:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000233:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000234:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000235:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000236:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000237:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000238:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000239:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000240:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x00000241:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000242:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000243:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000244:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000245:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000246:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000247:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000248:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000249:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000250:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000251:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000252:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000253:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000254:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000255:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000256:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000257:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000258:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000259:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000260:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x00000261:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000262:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000263:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000264:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000265:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000266:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000267:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000268:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000269:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000270:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000271:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000272:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000273:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000274:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000275:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000276:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000277:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000278:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000279:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000280:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x00000281:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000282:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000283:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000284:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000285:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000286:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000287:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000288:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000289:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000290:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000291:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000292:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000293:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000294:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000295:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000296:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000297:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000298:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000299:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000300:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)");
                        break;
                    }
                case 0x00000301:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000302:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000303:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000304:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000305:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000306:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000307:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000308:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000309:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000310:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000311:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000312:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000313:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000314:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000315:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000316:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000317:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000318:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000319:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000320:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x00000321:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000322:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000323:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000324:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000325:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000326:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000327:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000328:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000329:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000330:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000331:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000332:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000333:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000334:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000335:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000336:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000337:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000338:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000339:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000340:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x00000341:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000342:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000343:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000344:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000345:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000346:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000347:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000348:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000349:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000350:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000351:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000352:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000353:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000354:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000355:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000356:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000357:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000358:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000359:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000360:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x00000361:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000362:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000363:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000364:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000365:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000366:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000367:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000368:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000369:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000370:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000371:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000372:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000373:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000374:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000375:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000376:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000377:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000378:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000379:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000380:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x00000381:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000382:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000383:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000384:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000385:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000386:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000387:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000388:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000389:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000390:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x00000391:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x00000392:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000393:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x00000394:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000395:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000396:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000397:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x00000398:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00000399:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000000:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)");
                        break;
                    }
                case 0x40000001:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000002:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000003:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000004:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000005:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000006:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000007:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000008:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000009:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000010:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000011:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000012:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000013:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000014:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000015:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000016:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000017:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000018:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000019:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000020:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x40000021:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000022:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000023:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000024:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000025:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000026:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000027:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000028:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000029:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000030:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000031:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000032:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000033:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000034:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000035:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000036:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000037:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000038:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000039:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000040:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x40000041:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000042:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000043:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000044:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000045:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000046:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000047:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000048:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000049:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000050:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000051:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000052:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000053:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000054:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000055:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000056:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000057:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000058:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000059:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000060:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x40000061:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000062:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000063:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000064:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000065:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000066:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000067:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000068:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000069:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000070:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000071:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000072:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000073:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000074:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000075:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000076:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000077:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000078:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000079:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000080:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x40000081:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000082:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000083:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000084:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000085:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000086:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000087:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000088:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000089:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000090:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000091:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000092:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000093:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000094:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000095:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000096:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000097:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000098:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000099:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000100:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)");
                        break;
                    }
                case 0x40000101:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000102:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000103:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000104:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000105:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000106:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000107:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000108:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000109:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000110:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000111:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000112:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000113:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000114:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000115:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000116:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000117:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000118:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000119:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000120:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x40000121:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000122:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000123:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000124:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000125:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000126:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000127:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000128:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000129:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000130:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000131:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000132:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000133:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000134:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000135:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000136:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000137:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000138:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000139:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000140:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x40000141:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000142:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000143:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000144:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000145:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000146:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000147:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000148:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000149:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000150:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000151:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000152:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000153:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000154:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000155:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000156:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000157:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000158:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000159:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000160:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x40000161:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000162:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000163:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000164:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000165:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000166:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000167:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000168:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000169:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000170:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000171:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000172:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000173:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000174:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000175:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000176:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000177:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000178:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000179:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000180:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x40000181:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000182:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000183:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000184:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000185:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000186:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000187:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000188:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000189:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000190:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000191:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000192:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000193:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000194:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000195:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000196:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000197:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000198:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000199:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000200:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)");
                        break;
                    }


                case 0x40000201:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000202:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000203:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000204:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000205:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000206:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000207:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000208:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000209:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000210:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000211:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000212:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000213:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000214:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000215:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000216:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000217:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000218:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000219:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000220:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x40000221:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000222:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000223:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000224:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000225:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000226:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000227:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000228:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000229:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000230:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000231:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000232:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000233:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000234:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000235:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000236:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000237:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000238:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000239:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000240:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x40000241:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000242:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000243:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000244:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000245:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000246:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000247:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000248:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000249:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000250:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000251:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000252:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000253:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000254:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000255:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000256:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000257:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000258:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000259:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000260:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x40000261:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000262:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000263:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000264:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000265:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000266:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000267:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000268:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000269:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000270:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000271:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000272:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000273:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000274:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000275:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000276:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000277:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000278:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000279:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000280:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x40000281:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000282:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000283:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000284:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000285:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000286:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000287:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000288:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000289:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000290:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000291:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000292:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000293:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000294:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000295:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000296:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000297:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000298:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000299:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000300:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)");
                        break;
                    }
                case 0x40000301:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000302:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000303:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000304:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000305:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000306:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000307:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000308:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000309:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000310:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000311:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000312:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000313:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000314:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000315:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000316:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000317:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000318:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000319:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000320:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x40000321:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000322:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000323:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000324:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000325:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000326:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000327:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000328:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000329:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000330:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000331:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000332:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000333:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000334:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000335:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000336:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000337:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000338:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000339:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000340:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x40000341:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000342:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000343:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000344:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000345:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000346:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000347:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000348:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000349:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000350:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000351:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000352:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000353:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000354:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000355:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000356:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000357:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000358:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000359:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000360:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x40000361:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000362:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000363:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000364:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000365:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000366:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000367:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000368:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000369:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000370:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000371:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000372:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000373:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000374:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000375:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000376:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000377:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000378:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000379:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000380:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x40000381:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000382:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000383:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000384:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000385:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000386:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000387:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000388:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000389:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000390:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x40000391:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x40000392:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000393:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x40000394:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000395:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000396:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000397:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x40000398:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x40000399:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_HARD_DISK) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000000:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)");
                        break;
                    }
                case 0x80000001:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000002:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000003:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000004:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000005:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000006:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000007:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000008:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000009:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000010:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000011:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000012:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000013:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000014:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000015:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000016:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000017:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000018:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000019:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000020:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x80000021:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000022:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000023:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000024:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000025:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000026:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000027:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000028:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000029:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000030:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000031:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000032:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000033:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000034:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000035:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000036:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000037:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000038:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000039:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000040:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x80000041:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000042:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000043:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000044:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000045:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000046:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000047:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000048:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000049:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000050:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000051:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000052:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000053:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000054:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000055:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000056:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000057:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000058:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000059:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000060:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x80000061:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000062:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000063:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000064:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000065:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000066:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000067:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000068:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000069:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000070:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000071:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000072:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000073:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000074:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000075:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000076:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000077:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000078:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000079:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000080:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x80000081:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000082:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000083:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000084:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000085:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000086:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000087:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000088:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000089:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000090:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000091:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000092:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000093:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000094:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000095:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000096:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000097:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000098:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000099:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000100:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)");
                        break;
                    }
                case 0x80000101:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000102:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000103:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000104:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000105:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000106:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000107:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000108:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000109:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000110:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000111:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000112:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000113:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000114:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000115:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000116:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000117:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000118:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000119:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000120:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x80000121:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000122:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000123:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000124:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000125:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000126:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000127:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000128:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000129:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000130:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000131:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000132:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000133:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000134:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000135:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000136:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000137:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000138:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000139:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000140:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x80000141:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000142:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000143:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000144:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000145:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000146:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000147:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000148:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000149:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000150:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000151:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000152:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000153:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000154:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000155:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000156:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000157:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000158:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000159:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000160:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x80000161:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000162:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000163:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000164:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000165:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000166:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000167:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000168:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000169:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000170:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000171:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000172:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000173:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000174:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000175:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000176:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000177:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000178:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000179:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000180:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x80000181:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000182:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000183:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000184:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000185:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000186:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000187:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000188:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000189:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000190:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000191:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000192:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000193:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000194:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000195:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000196:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000197:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000198:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000199:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000200:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)");
                        break;
                    }


                case 0x80000201:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000202:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000203:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000204:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000205:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000206:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000207:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000208:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000209:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000210:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000211:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000212:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000213:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000214:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000215:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000216:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000217:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000218:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000219:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000220:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x80000221:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000222:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000223:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000224:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000225:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000226:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000227:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000228:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000229:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000230:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000231:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000232:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000233:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000234:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000235:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000236:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000237:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000238:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000239:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000240:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x80000241:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000242:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000243:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000244:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000245:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000246:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000247:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000248:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000249:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000250:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000251:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000252:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000253:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000254:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000255:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000256:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000257:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000258:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000259:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000260:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x80000261:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000262:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000263:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000264:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000265:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000266:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000267:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000268:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000269:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000270:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000271:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000272:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000273:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000274:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000275:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000276:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000277:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000278:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000279:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000280:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x80000281:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000282:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000283:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000284:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000285:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000286:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000287:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000288:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000289:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000290:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000291:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000292:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000293:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000294:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000295:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000296:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000297:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000298:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000299:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000300:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)");
                        break;
                    }
                case 0x80000301:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000302:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000303:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000304:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000305:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000306:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000307:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000308:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000309:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000310:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000311:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000312:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000313:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000314:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000315:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000316:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000317:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000318:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000319:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000320:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x80000321:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000322:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000323:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000324:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000325:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000326:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000327:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000328:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000329:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000330:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000331:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000332:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000333:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000334:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000335:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000336:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000337:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000338:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000339:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000340:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)");
                        break;
                    }
                case 0x80000341:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000342:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000343:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000344:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000345:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000346:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000347:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000348:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000349:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000350:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000351:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000352:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000353:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000354:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000355:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000356:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000357:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000358:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000359:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000360:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)");
                        break;
                    }
                case 0x80000361:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000362:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000363:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000364:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000365:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000366:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000367:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000368:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000369:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000370:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000371:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000372:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000373:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000374:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000375:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000376:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000377:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000378:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000379:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_5_RW)  (MEDIA_TYPE_DVD_9_RO)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000380:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)");
                        break;
                    }
                case 0x80000381:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000382:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000383:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000384:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000385:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000386:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000387:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000388:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000389:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000390:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)");
                        break;
                    }
                case 0x80000391:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)");
                        break;
                    }
                case 0x80000392:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000393:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)");
                        break;
                    }
                case 0x80000394:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000395:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000396:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000397:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_DVD_X2)  (MEDIA_TYPE_DVD_CD)");
                        break;
                    }
                case 0x80000398:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x80000399:
                    {
                        t.AppendText(" (MEDIA_TYPE_NONSECURE_MODE) (MEDIA_TYPE_MEDIA_BOARD)  (MEDIA_TYPE_DONGLE)  (MEDIA_TYPE_DVD_9_RW)  (MEDIA_TYPE_DVD_5_RO)  (MEDIA_TYPE_HARD_DISK)  (MEDIA_TYPE_CD)");
                        break;
                    }
                case 0x00FFFFFF:
                    {
                        t.AppendText(" (MEDIA_TYPE_MEDIA_MASK)");
                        break;
                    }

            }
        }
        static string[] game_rating_table = new string[0x7] {" (GAME_RATING_RATED_PENDING)"," (GAME_RATING_ADULTS_ONLY)"," (GAME_RATING_MATURE)"," (GAME_RATING_TEEN)"," (GAME_RATING_EVERYONE)"," (GAME_RATING_KIDS_TO_ADULTS)"," (GAME_RATING_EARLY_CHILDHOOD)" };
        public static void FetchGameRating(uint Addr){
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            if (Addr.ToString("X8") != null)
            {
                try
                {
                    t.AppendText(game_rating_table[Convert.ToInt32(Addr.ToString("X8"))]);
                }
                catch (Exception)
                {
                    t.AppendText(" (UNDEFINED)");
                }
            }
            else { 
                t.AppendText(" (GAME_RATING_RATED PENDING)"); 
            }
        }

        public static void FetchGameRegion(uint t_offset)
        {
                RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
                switch (t_offset)
                {
                    case 0x00000000:
                        t.AppendText(" (UNDEFINED)");
                        break;
                    case 0x00000001:
                        t.AppendText(" (GAME_REGION_NA)");
                        break;
                    case 0x00000002:
                        t.AppendText(" (GAME_REGION_JAPAN)");
                        break;
                    case 0x00000003:
                        t.AppendText(" (GAME_REGION_JAPAN)  (GAME_REGION_NA)");
                        break;
                    case 0x00000004:
                        t.AppendText(" (GAME_REGION_RESTOFWORLD)");
                        break;
                    case 0x00000005:
                        t.AppendText(" (GAME_REGION_NA)  (GAME_REGION_RESTOFWORLD)");
                        break;
                    case 0x00000006:
                        t.AppendText(" (GAME_REGION_JAPAN) (GAME_REGION_RESTOFWORLD)");
                        break;
                    case 0x00000007:
                        t.AppendText(" (GAME_REGION_NA)  (GAME_REGION_JAPAN)  (GAME_REGION_RESTOFWORLD)");
                        break;
                    case 0x80000000:
                        t.AppendText(" (GAME_REGION_MANUFACTURING)");
                        break;
                    case 0x80000001:
                        t.AppendText(" (GAME_REGION_MANUFACTURING)  (GAME_REGION_NA)");
                        break;
                    case 0x80000002:
                        t.AppendText(" (GAME_REGION_MANUFACTURING)  (GAME_REGION_JAPAN)");
                        break;
                    case 0x80000003:
                        t.AppendText(" (GAME_REGION_MANUFACTURING)  (GAME_REGION_JAPAN)  (GAME_REGION_NA)");
                        break;
                    case 0x80000004:
                        t.AppendText(" (GAME_REGION_MANUFACTURING)  (GAME_REGION_RESTOFWORLD)");
                        break;
                    case 0x80000005:
                        t.AppendText(" (GAME_REGION_MANUFACTURING)  (GAME_REGION_NA)  (GAME_REGION_RESTOFWORLD)");
                        break;
                    case 0x80000006:
                        t.AppendText(" (GAME_REGION_MANUFACTURING)  (GAME_REGION_JAPAN) (GAME_REGION_RESTOFWORLD)");
                        break;
                    case 0x80000007:
                        t.AppendText(" (GAME_REGION_MANUFACTURING)  (GAME_REGION_NA)  (GAME_REGION_JAPAN)  (GAME_REGION_RESTOFWORLD)");
                        break;
                }
        }
  
        static string[] init_flags_table = new string[0xF] {" (MountUtilityDrive)", " (FormatUtilityDrive)"," (MountUtilityDrive)  (FormatUtilityDrive)"," (Limit64Megabytes)"," (MountUtilityDrive)  (Limit64Megabytes)","  (FormatUtilityDrive)  (Limit64Megabytes)","(MountUtilityDrive)  (FormatUtilityDrive)  (Limit64Megabytes)" ," (DontSetupHarddisk)" ," (MountUtilityDrive)  (DontSetupHarddisk)","  (FormatUtilityDrive)  (DontSetupHarddisk)","(MountUtilityDrive)   (FormatUtilityDrive)  (DontSetupHarddisk)", " (Limit64Megabytes)  (DontSetupHarddisk)","(MountUtilityDrive)  (Limit64Megabytes)  (DontSetupHarddisk)","(FormatUtilityDrive)  (Limit64Megabytes)  (DontSetupHarddisk)","(MountUtilityDrive)  (FormatUtilityDrive)  (Limit64Megabytes)  (DontSetupHarddisk)"};
        public static void FetchInitFlags(int t_offset)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            try {
                t.AppendText(init_flags_table[Convert.ToInt32(t_offset.ToString("X8")) - 1]);
            } catch (Exception){
                t.AppendText(" (UNKNOWN)" + "\r\n");
            } 
        }
      
        public static string FetchImageHeaderOffset(long t_offset, int t_size, int type)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            byte[] addr = new byte[t_size];
            char[] addrChar = new char[0x1000];
            string buffer = null;

            switch (type)
            {
                case 0:
                        for (int i = 0; i < t_size; i++)
                        {
                            addr[i] = mem[t_offset + i];
                        }

                        Array.Reverse(addr);

                        for (int i = 0; i < t_size; i++) {
                            buffer += string.Format("{0:X2}", addr[i]);
                        }
                        break;
                case 1:
                        for (int i = 0; i < 0x0100; i++) { 
                            addrChar[i] = (char)mem[t_offset + i]; if (addrChar[i] == '\0') 
                            i = 0x0100; 
                        }
                        for (int i = 0; i < 0x0100; i++) { 
                            if (!(addrChar[i] == '\0' | addrChar[i] == '0')) { 
                                buffer += string.Format("{0}", (char)addrChar[i]); 
                            }
                        }
                        break;
                case 2: //Used for Certificate Title Name.
                        for (int i = 0; i < t_size; i++) { 
                            addrChar[i] = (char)mem[t_offset + i]; 
                        }
                        for (int i = 0; i < t_size; i++) { 
                            if (!(addrChar[i] == '\0' | addrChar[i] == '0')) {
                                buffer += string.Format("{0}", (char)addrChar[i]);
                            } 
                        }
                        break;
                case 3: //Used for Debug Pathname.
                        for (int i = 0; i < 0x0100; i++) { 
                            addrChar[i] = (char)mem[t_offset + i]; 
                            if (addrChar[i] == ':' | addrChar[i] == '0'){ 
                                i = 0x0100; 
                            }
                        }

                        for (int i = 0; i < 0x0100; i++) { 
                            if (!(addrChar[i] == '\0' | addrChar[i] == '0')) { 
                                buffer += string.Format("{0}", (char)addrChar[i]); 
                            }
                        }
                        if (buffer != null) {
                            buffer = buffer.ToString().Remove(buffer.Length - 2);
                        } 
                        break;
            }
            offset += t_size;
            return buffer;
        }

        static string[] sect_flags_table = new string[0x27] {" (Writable)"," (Preload)"," (Writable)  (Preload)"," (Executable)",
       " (Writable)  (Executable)"," (Preload)  (Executable)"," (Writable)  (Preload)  (Executable)","(Inserted File)", 
       " (Writable)  (Inserted File)"," (Head Page Read Only) "," (Writable)  (Head Page Read Only) "," (Head Page Read Only)  (Preload)",
       " (Writable)  (Head Page Read Only)  (Preload)"," (Executable)  (Head Page Read Only)"," (Writable)  (Executable)  (Head Page Read Only)",
       " (Preload)  (Executable)  (Head Page Read Only)"," (Writable)  (Preload)  (Executable)  (Head Page Read Only)","(Inserted File)  (Head Page Read Only)", 
       " (Writable)  (Inserted File)  (Head Page Read Only)",
       " (Tail Page Read Only)"," (Writable)"," (Preload)  (Tail Page Read Only)"," (Writable)  (Preload)  (Tail Page Read Only)"," (Executable)  (Tail Page Read Only)",
       " (Writable)  (Executable)  (Tail Page Read Only)"," (Preload)  (Executable)  (Tail Page Read Only)"," (Writable)  (Preload)  (Executable)  (Tail Page Read Only)","(Inserted File)  (Tail Page Read Only)", 
       " (Writable)  (Inserted File)  (Tail Page Read Only)",
       " (Head Page Read Only)  (Tail Page Read Only)"," (Writable)  (Head Page Read Only)  (Tail Page Read Only)"," (Preload)"," (Writable)  (Preload)  (Head Page Read Only)  (Tail Page Read Only)"," (Executable)  (Head Page Read Only)  (Tail Page Read Only)",
       " (Writable)  (Executable)  (Head Page Read Only)  (Tail Page Read Only)"," (Preload)  (Executable)  (Head Page Read Only)  (Tail Page Read Only)"," (Writable)  (Preload)  (Executable)  (Head Page Read Only)  (Tail Page Read Only)","(Inserted File)  (Head Page Read Only)  (Tail Page Read Only)", 
       " (Writable)  (Inserted File)  (Head Page Read Only)  (Tail Page Read Only)"};

        public static void FetchSectionOffset(int t_offset, int t_size)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            for (int j = 0; j < Convert.ToInt32(num_sections, 16); j++)
            {
                sect_flags[j] = Convert.ToInt32(FetchImageHeaderOffset(offset, 0x0004, 0), 16);

                t.AppendText("             Section Flags: 0x" + sect_flags[j].ToString("X8") );
                try {
                    t.AppendText(sect_flags_table[Convert.ToInt32(sect_flags[j].ToString("X8")) - 1]);
                } catch (Exception) {
                    t.AppendText(" (UNKNOWN)" + "\r\n"); 
                }
         
                sect_virtual_addr[j] = FetchImageHeaderOffset(offset, 0x0004, 0);
                t.AppendText("\r\n" + "             Virtual Address: 0x" + sect_virtual_addr[j].ToString() + "\r\n");

                sect_virtual_size[j] = FetchImageHeaderOffset(offset, 0x0004, 0);
                t.AppendText("             Virtual Size: 0x" + sect_virtual_size[j].ToString() + "\r\n");

                sect_raw_addr[j] = FetchImageHeaderOffset(offset, 0x0004, 0);
                t.AppendText("             Raw Address: 0x" + sect_raw_addr[j].ToString() + "\r\n");

                sect_raw_size[j] = FetchImageHeaderOffset(offset, 0x0004, 0);
                t.AppendText("             Raw Size: 0x" + sect_raw_size[j].ToString() + "\r\n");

                sect_name_addr[j] = FetchImageHeaderOffset(offset, 0x0004, 0);
                t.AppendText("             Section Name Address: 0x" + sect_name_addr[j].ToString());

                int sub_offset = SubtractOffset(sect_name_addr[j], base_addr);
                t.AppendText(" (0x" + sub_offset.ToString("X8") + ")");

                t.AppendText(" (" +"\"");
                for (int i = 0; i < 14; i++)
                {
                    sect_name_addr[i] = Convert.ToString((char)mem[sub_offset + i]);
                    if (!(sect_name_addr[i] =="\0" | sect_name_addr[i] =="Ì"))
                    {
                        sect_name_str[i] = sect_name_addr[i];
                        t.AppendText(sect_name_str[i].ToString());
                        //Debug.Print("\r\n" +"k Offset:" + Offset.ToString("X8") +"  symbol:" + (char)pXbe[Offset] +"   " + pXbe[Offset].ToString());
                    }
                    else
                    {
                        i = 14;
                    }
                }
                t.AppendText("\"" + ")" + "\r\n");

                sect_name_ref_count[j] = FetchImageHeaderOffset(offset, 0x0004, 0);
                t.AppendText("             Section Name Reference Count: 0x" + sect_name_ref_count[j].ToString() + "\r\n");

                sect_head_shared_ref_count_addr[j] = FetchImageHeaderOffset(offset, 0x0004, 0);
                t.AppendText("             Head Shared Page Reference Count Address: 0x" + sect_head_shared_ref_count_addr[j].ToString() + "\r\n");

                sect_tail_shared_ref_count_addr[j] = FetchImageHeaderOffset(offset, 0x0004, 0);
                t.AppendText("             Tail Shared Page Reference Count Address: 0x" + sect_tail_shared_ref_count_addr[j].ToString() + "\r\n");

                sect_digest[j] = FetchImageHeaderOffset(offset, 0x0014, 0);
                t.AppendText("             Section Digest: 0x" + sect_digest[j].ToString() + "\r\n" + "\r\n");
            }
        }

        public static string getSectionName(int ia32_pc)
        {
            string sect_name = "";
            for (int i = 0; i < xboxhle.xbe.sect_raw_addr.Length; i++)
            {
                if (ia32_pc >= Convert.ToInt32(xboxhle.xbe.sect_raw_addr[i]))
                {
                    sect_name += xboxhle.xbe.sect_name_str[i];
                }
                else if (ia32_pc <= Convert.ToInt32(xboxhle.xbe.sect_raw_addr[i]))
                {
                    sect_name += xboxhle.xbe.sect_name_str[i];
                }
            }
            return sect_name;
        }

        public static int FetchEntryPoint(int t_offset)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            if (t_offset.ToString("X").Remove(2) == "94")
            {
                offset = XorOffset(entry_points, Convert.ToString(XOR_ENTRY_DEBUG));
                t.AppendText(" (Debug: 0x" + offset.ToString("X8") + ")" + "\r\n");
            }
            else if (t_offset.ToString("X").Remove(2) == "A8")
            {
                offset = XorOffset(entry_points, Convert.ToString(XOR_ENTRY_RETAIL));
                t.AppendText(" (Retail: 0x" + offset.ToString("X8") + ")" + "\r\n");
            }
            return offset;
        }
        public static void FetchKernelThunk(int t_offset)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            if (t_offset.ToString("X").Remove(2) == "EF")
            {
                offset = XorOffset(kernel_thunk_addr, XOR_KERNEL_DEBUG);
                t.AppendText(" (Debug: 0x" + offset.ToString("X8") + ")" + "\r\n");
            }
            else if (t_offset.ToString("X").Remove(2) == "5B")
            {
                offset = XorOffset(kernel_thunk_addr, XOR_KERNEL_RETAIL);
                t.AppendText(" (Retail: 0x" + offset.ToString("X8") + ")" + "\r\n");
            }
        } 
        public static void FetchTLSAddr(int t_offset)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            t.AppendText(" (0x" + offset.ToString("X8") + ")" + "\r\n");

            for (int k = 0; k < Convert.ToInt32(tls_addr, 16); k++)
            {
                //Used for detecting patterns so that we can retrieve the correct TLS Offset Address.
                if (mem[offset + 0] == 0 && mem[offset + 1] == 0 && mem[offset + 2] == 0 && mem[offset + 3] == 0 && mem[offset + 4] == 0 && mem[offset + 5] == 0 && mem[offset + 6] == 0 && mem[offset + 7] == 0 && mem[offset + 8] > 0 | mem[offset + 8] == 0 && mem[offset + 9] > 0 && mem[offset + 10] > 0 && mem[offset + 11] == 0 && mem[offset + 12] == 0 && mem[offset + 13] == 0 && mem[offset + 14] == 0 && mem[offset + 15] == 0 && mem[offset + 16] > 0 && mem[offset + 17] == 0 && mem[offset + 18] == 0 && mem[offset + 19] == 0 && mem[offset + 20] == 0 && mem[offset + 21] == 0 && mem[offset + 22] == 0 && mem[offset + 23] == 0)
                {
                    //Debug.Print("\r\n" +"k Offset:" + Offset.ToString("X8") +"  symbol:" + (char)mem[Offset] +"   " + mem[Offset].ToString());
                    k = Convert.ToInt32(tls_addr, 16);
                    offset -= 0x0004;
                }
                offset += 0x0004;
                if (offset == Convert.ToInt32(tls_addr, 16))
                {
                    k = Convert.ToInt32(tls_addr, 16);
                }
            }
            for (int q = 0; q < Convert.ToInt32(tls_addr, 16); q++)
            {                    
                //Used for detecting patterns so that we can retrieve the correct TLS Offset Address.
                try
                {
                    if (mem[offset + 0] == 0 && mem[offset + 1] == 0 && mem[offset + 2] == 0 && mem[offset + 3] == 0 && mem[offset + 4] == 0 && mem[offset + 5] == 0 && mem[offset + 6] == 0 && mem[offset + 7] == 0 && mem[offset + 8] > 0 | mem[offset + 8] == 0 && mem[offset + 9] > 0 && mem[offset + 10] > 0 && mem[offset + 11] == 0 && mem[offset + 12] == 0 && mem[offset + 13] == 0 && mem[offset + 14] == 0 && mem[offset + 15] == 0 && mem[offset + 16] > 0 && mem[offset + 17] == 0 && mem[offset + 18] == 0 && mem[offset + 19] == 0 && mem[offset + 20] == 0 && mem[offset + 21] == 0 && mem[offset + 22] == 0 && mem[offset + 23] == 0)
                    {
                        //Debug.Print("\r\n" +"q Offset:" + Offset.ToString("X8") +"  symbol:" + (char)mem[Offset] +"   " + mem[Offset].ToString());
                        q = Convert.ToInt32(tls_addr, 16);
                        offset += 0x0004;
                    }
                    offset -= 0x0004;
                }           
                catch (Exception)
                {
                    offset = t_offset;
                    q = Convert.ToInt32(tls_addr, 16);
                }     
            }

            tls_data_start_addr  = FetchImageHeaderOffset(offset, 0x0004, 0);
            t.AppendText("             TLS Data Start Address: 0x" + tls_data_start_addr + "\r\n");

            tls_data_end_addr  = FetchImageHeaderOffset(offset, 0x0004, 0);
            t.AppendText("             TLS Data End Address: 0x" + tls_data_end_addr + "\r\n");

            tls_index_addr  = FetchImageHeaderOffset(offset, 0x0004, 0);
            t.AppendText("             TLS Index Address: 0x" + tls_index_addr + "\r\n");

            tls_callback_addr  = FetchImageHeaderOffset(offset, 0x0004, 0);
            t.AppendText("             TLS Callback Address: 0x" + tls_callback_addr + "\r\n");

            tls_size_zero_fill  = FetchImageHeaderOffset(offset, 0x0004, 0);
            t.AppendText("             Size of Zero Fill: 0x" + tls_size_zero_fill + "\r\n");

            tls_chars  = FetchImageHeaderOffset(offset, 0x0004, 0);
            t.AppendText("             Characteristics: 0x" + tls_chars + "\r\n" + "\r\n");
        }
        public static string MajorMinorBuildVersionAddress(int t_offset, int t_size)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            byte[] addr = new byte[t_size];
            char[] addrChar = new char[t_size];
            string buffer = null;
            switch (t_size)
            {
                case 0x0002:
                    for (int i = 0; i < 2; i++) { 
                        addr[i] = mem[t_offset + i]; 
                    }

                    Array.Reverse(addr);

                    for (int i = 0; i < 2; i++) { 
                        buffer += string.Format("{0:X2}", addr[i]); 
                    }
                    break;
                case 0x0008:
                    for (int i = 0; i < 8; i++) addrChar[i] = (char)mem[t_offset + i];

                    for (int i = 0; i < 8; i++) if (!(addrChar[i] == '\0' | addrChar[i] == '0'))
                    {
                            buffer += string.Format("{0}", (char)addrChar[i]);
                    }
                    break;
            }
            return buffer;
        }

        public static void FetchKernelLibraryVersion(int t_offset)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            string buffer = null;
            int w = 0;
            for (int j = 0; j < Convert.ToInt32(num_lib_versions, 16); j++)
            {
                w += 1;

                lib_name = MajorMinorBuildVersionAddress(t_offset, 0x0008);
                t_offset += 0x0008;

                lib_major_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t_offset += 0x0002;

                lib_minor_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t_offset += 0x0002;

                lib_build_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t_offset += 0x0002;

                lib_vers_addr = buffer = string.Format("{0:D}.{1:D}.{2:D}", Convert.ToInt32(lib_major_version, 16), Convert.ToInt32(lib_minor_version, 16), Convert.ToInt32(lib_build_version, 16));
                t_offset += 0x0002;

                lib_flags = MajorMinorBuildVersionAddress(t_offset, 0x0002);

                if (!(Convert.ToInt32(lib_major_version, 16) == 1 && Convert.ToInt32(lib_minor_version, 16) == 0 && Convert.ToInt32(lib_build_version, 16) < 10000))
                {
                    
                    for (int i = 0; i < w; i++)
                    {
                        t_offset -= 0x0008;
                        t_offset -= 0x0002;
                        t_offset -= 0x0002;
                        t_offset -= 0x0002;
                        t_offset -= 0x0002;
                    }
                    j = Convert.ToInt32(num_lib_versions, 16);
                }
            }
            for (int k = 0; k < w - 1; k++)
            {
                lib_name = MajorMinorBuildVersionAddress(t_offset, 0x0008);

                t_offset += 0x0008;
                if (lib_name == "XBOXKRNL" | lib_name == "LIBC")
                {
                    t.AppendText("\r\n" + "            " + "Library Name:" + lib_name);
                    lib_major_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                    t.AppendText("\r\n" + "            " + "Major Version: 0x" + lib_major_version);
                    t_offset += 0x0002;

                    lib_minor_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                    t.AppendText("\r\n" + "            " + "Minor Version: 0x" + lib_minor_version);
                    t_offset += 0x0002;

                    lib_build_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                    t.AppendText("\r\n" + "            " + "Build Version: 0x" + lib_build_version);

                    lib_vers_addr = buffer = string.Format("{0:D}.{1:D}.{2:D}", Convert.ToInt32(lib_major_version, 16), Convert.ToInt32(lib_minor_version, 16), Convert.ToInt32(lib_build_version, 16));
                    t.AppendText("\r\n" + "            " + "Version:" + buffer);
                    t_offset += 0x0002;

                    lib_flags = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                    t.AppendText("\r\n" + "            " + "Library Flags: 0x" + lib_flags + "\r\n\r\n");
                    t_offset += 0x0002;

                }
            }
        }
        public static void FetchLibraryVersion(int t_offset) 
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            string buffer = null;
            for (int j = 0; j < Convert.ToInt32(num_lib_versions, 16); j++)
            {
                lib_name = MajorMinorBuildVersionAddress(t_offset, 0x0008);
                t.AppendText("\r\n" + "            " + "Library Name: " + lib_name);
                t_offset += 0x0008;

                lib_major_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t.AppendText("\r\n" + "            " + "Major Version: 0x" + lib_major_version);
                t_offset += 0x0002;

                lib_minor_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t.AppendText("\r\n" + "            " + "Minor Version: 0x" + lib_minor_version);
                t_offset += 0x0002;

                lib_build_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t.AppendText("\r\n" + "            " + "Build Version: 0x" + lib_build_version);

                lib_vers_addr = buffer = string.Format("{0:D}.{1:D}.{2:D}", Convert.ToInt32(lib_major_version, 16), Convert.ToInt32(lib_minor_version, 16), Convert.ToInt32(lib_build_version, 16));
                t.AppendText("\r\n" + "            " + "Version: " + buffer);
                t_offset += 0x0002;

                lib_flags = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t.AppendText("\r\n" + "            " + "Library Flags: 0x" + lib_flags + "\r\n\r\n");
                t_offset += 0x0002;
            }
        }

        public static void FetchXAPIVersion(int t_offset)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            string buffer = null;
            for (int j = 0; j < 1; j++)
            {
                lib_name = MajorMinorBuildVersionAddress(t_offset, 0x0008);
                t.AppendText("\r\n" + "            " + "Library Name: " + lib_name);
                t_offset += 0x0008;

                lib_major_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t.AppendText("\r\n" + "            " + "Major Version: 0x" + lib_major_version);
                t_offset += 0x0002;

                lib_minor_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t.AppendText("\r\n" + "            " + "Minor Version: 0x" + lib_minor_version);
                t_offset += 0x0002;

                lib_build_version = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t.AppendText("\r\n" + "            " + "Build Version: 0x" + lib_build_version);

                lib_vers_addr = buffer = string.Format("{0:D}.{1:D}.{2:D}", Convert.ToInt32(lib_major_version, 16), Convert.ToInt32(lib_minor_version, 16), Convert.ToInt32(lib_build_version, 16));
                t.AppendText("\r\n" + "            " + "Version: " + buffer + "\r\n");

                t_offset += 0x0002;
                lib_flags = MajorMinorBuildVersionAddress(t_offset, 0x0002);
                t.AppendText("\r\n" + "            " + "Library Flags: 0x" + lib_flags + "\r\n\r\n");
                t_offset += 0x0002;
            }
        }
        public static void FetchDateTime(int t_offset)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(t_offset);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            t.AppendText(" (" + dt + ")");
        }

        public static void filesize_pXbe(long fsize)
        {
            RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
            decimal File_Length_Result;
            const decimal _Byte = 8;
            const decimal _Kilobyte = 1024;
            const decimal _Megabyte = 1048576;
            const decimal _Gigabyte = 1073741824;
            string buffer = null;
            if (fsize > _Gigabyte)
            {
                File_Length_Result = fsize / _Megabyte;

                buffer = string.Format("{0}", File_Length_Result);

                if (buffer.Length > 4) t.AppendText(" (" + buffer.Remove(4) + " GB)" + "\r\n"); else t.AppendText(" (" + buffer + " GB)" + "\r\n");
            }
            else if (fsize > _Megabyte && fsize < _Gigabyte)
            {
                File_Length_Result = fsize / _Megabyte;

                buffer = string.Format("{0}", File_Length_Result);

                if (buffer.Length > 4) t.AppendText(" (" + buffer.Remove(4) + " MB)" + "\r\n"); else t.AppendText(" (" + buffer + " MB)" + "\r\n");
            }
            else if (fsize > _Kilobyte && fsize < _Megabyte)
            {
                File_Length_Result = fsize / _Kilobyte;

                buffer = string.Format("{0}", File_Length_Result);

                if (buffer.Length > 4) t.AppendText(" (" + buffer.Remove(4) + " KB)" + "\r\n"); else t.AppendText(" (" + buffer + " KB)" + "\r\n");
            }
            else if (fsize > _Byte && fsize < _Kilobyte) t.AppendText("\r\n");
        }


        static public void load_pXbe(string fname)
        {
            pXbe = new System.IO.FileStream(fname, System.IO.FileMode.Open);


            pXbe.Read(mem, 0, (int)pXbe.Length);
            //int i = 0;
            //while (i < pXbe.Length)
            //{
            //    mem[i] = (byte)pXbe.ReadByte();
            //    i++;
            //}
        }

        public static void print_pXbe()
        {
            //Retrieve the first 4 bytes to our signature, which is the Magic Number for validating XBE Files.
            magic_num = Convert.ToString(FetchImageHeaderOffset(0x0, 0x0004, 0));
            
            //Used for Error Checking.
            try
            {
                RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
                //We check to see if the buffer matches with our Constant Magic number, if it does then we proceed with dumping the contents of our XBE File.
                if (magic_num == Convert.ToString(_magic_num.ToString("X8")))
                {
                    t.AppendText("XBOX Executable Filename: " + pXbe.Name + "\r\n");

                    t.AppendText("XBE Filesize: " + pXbe.Length + " Bytes");

                    filesize_pXbe(pXbe.Length);

                    pXbe.Close();
                    
                    //Start of XBOX PE Header
                    magic_num = Convert.ToString(FetchImageHeaderOffset(0x0, 0x0004, 0)); //Reset our offset once more to print our Magic Number. 
                    t.AppendText("Magic Number 0x" + magic_num);

                    magic_num_str = Convert.ToString(FetchImageHeaderOffset(0x0, 0x0004, 2));
                    t.AppendText(" (" + magic_num_str + ")" + "\r\n");
                
                    auth_sig = FetchImageHeaderOffset(0x0004, 0x0100, 0); //After the first 4 bytes, we now retreive our Authenticated Signature.
                    t.AppendText("Authenticated Signature: " + auth_sig + "\r\n");
                    
                    base_addr = FetchImageHeaderOffset(0x0104, 0x0004, 0);
                    t.AppendText("Base Address: 0x" + base_addr + "\r\n");

                    size_header = FetchImageHeaderOffset(0x0108, 0x0004, 0);
                    t.AppendText("Size of Headers: 0x" + size_header + "\r\n");

                    size_image = FetchImageHeaderOffset(0x010c, 0x0004, 0);
                    t.AppendText("Size of Image: 0x" + size_image + "\r\n");

                    size_image_header = FetchImageHeaderOffset(0x0110, 0x0004, 0);
                    t.AppendText("Size of Image Headers: 0x" + size_image_header + "\r\n");

                    time_date = FetchImageHeaderOffset(0x0114, 0x0004, 0);
                    t.AppendText("Time&Date Stamp: 0x" + time_date);

                    if (!(Convert.ToInt32(time_date, 16) == 0x0))
                    {
                        FetchDateTime(Convert.ToInt32(time_date, 16));
                    }

                    //Start of our Certificate.
                    cert_addr = FetchImageHeaderOffset(0x0118, 0x0004, 0);
                    t.AppendText("\r\n" + "Certificate Address: 0x" + cert_addr);
                    
                    offset = SubtractOffset(cert_addr, base_addr);
                    t.AppendText(" (0x" + offset.ToString("X8") + ")" + "\r\n");

                    size_cert = FetchImageHeaderOffset(offset, 0x0004, 0);
                    t.AppendText("             Size of Certicate: 0x" + size_cert + "\r\n");

                    cert_timedate = FetchImageHeaderOffset(offset, 0x0004, 0);
                    t.AppendText("             Time&Date: 0x" + cert_timedate);

                    if (!(Convert.ToInt32(cert_timedate, 16) == 0x0))
                    {
                        FetchDateTime(Convert.ToInt32(cert_timedate, 16));
                    }

                    cert_title_id = FetchImageHeaderOffset(offset, 0x0004, 0);
                    t.AppendText("\r\n" + "             Title ID: 0x" + cert_title_id + "\r\n");

                    cert_title_name = FetchImageHeaderOffset(offset, 0x0050, 2);
                    if (!(cert_title_name == null))
                    {
                        t.AppendText("             Title Name: " + "L" + "\"" + cert_title_name.ToString() + "\"" + "\r\n");
                    }
                    else {
                        t.AppendText("             Title Name: " + "L" + "(UNDEFINED)" + "\r\n");

                    }

                    cert_alt_title_id = FetchImageHeaderOffset(offset, 0x0040, 0);
                    t.AppendText("             Alternate Title IDs: " + cert_alt_title_id + "\r\n");

                    cert_allowed_media = FetchImageHeaderOffset(offset, 0x0004, 0);
                    t.AppendText("             Allowed Media: 0x" + cert_allowed_media);

                    FetchAllowedMedia(Convert.ToUInt32(cert_allowed_media, 16));

                    cert_game_region = FetchImageHeaderOffset(offset, 0x0004, 0);
                    t.AppendText("\r\n" + "             Game Region: 0x" + cert_game_region);

                    FetchGameRegion(Convert.ToUInt32(cert_game_region, 16));
                    
                    cert_game_rating = FetchImageHeaderOffset(offset, 0x0004, 0);
                    t.AppendText("\r\n" + "             Game Rating: 0x" + cert_game_rating);

                    FetchGameRating(Convert.ToUInt32(cert_game_rating, 16));

                    cert_disk_num = FetchImageHeaderOffset(offset, 0x0004, 0);
                    t.AppendText("\r\n" + "             Disk Number: 0x" + cert_disk_num + "\r\n");

                    cert_version = FetchImageHeaderOffset(offset, 0x0004, 0);
                    t.AppendText("             Certificate Version: 0x" + cert_version + "\r\n");

                    cert_lan_key = FetchImageHeaderOffset(offset, 0x0010, 0);
                    t.AppendText("             Lan Key: " + cert_lan_key + "\r\n");

                    cert_sig_key = FetchImageHeaderOffset(offset, 0x0010, 0);
                    t.AppendText("             Signature Key: " + cert_sig_key + "\r\n");

                    cert_alt_sig_keys = FetchImageHeaderOffset(offset, 0x0100, 0);
                    t.AppendText("             Alternate Signature Keys: " + cert_alt_sig_keys + "\r\n");

                    num_sections = FetchImageHeaderOffset(0x011c, 0x0004, 0);
                    t.AppendText("Number of Sections: 0x" + num_sections);
                    
                    if (Convert.ToInt32(num_sections, 16) > 1)
                    {
                        t.AppendText(" (" + Convert.ToInt32(num_sections, 16).ToString("D") + " Sections)" + "\r\n");
                    }
                    else if (Convert.ToInt32(num_sections, 16) == 1)
                    {
                        t.AppendText(" (" + Convert.ToInt32(num_sections, 16).ToString("D") + " Section)" + "\r\n");
                    }                    
                    
                    sect_addr = FetchImageHeaderOffset(0x0120, 0x0004, 0);
                    t.AppendText("Section Headers Address: 0x" + sect_addr);

                    //Start of Section Header.
                    offset = SubtractOffset(sect_addr, base_addr);
                    t.AppendText(" (0x" + offset.ToString("X8") + ")" + "\r\n");

                    if (!(offset == 0x0))
                    {
                        FetchSectionOffset(offset, Convert.ToInt32(num_sections, 16));
                    }   

                    init_flags = FetchImageHeaderOffset(0x0124, 0x0004, 0);
                    t.AppendText("Initialization Flags: 0x" + init_flags);

                    if (!(Convert.ToInt32(init_flags, 16) == 0x0))
                    {
                        FetchInitFlags(Convert.ToInt32(init_flags, 16));
                    }

                    entry_points = FetchImageHeaderOffset(0x0128, 0x0004, 0);
                    t.AppendText("\r\n" + "Entry Point: 0x" + entry_points);

                    if (!(Convert.ToInt32(entry_points, 16) == 0x0))
                    {
                       entry_points = FetchEntryPoint(Convert.ToInt32(entry_points, 16)).ToString("X");
                    }

                    tls_addr = FetchImageHeaderOffset(0x012c, 0x0004, 0);
                    t.AppendText("TLS Address: 0x" + tls_addr);
                    offset = SubtractOffset(tls_addr, base_addr);
                    if (!(Convert.ToInt32(tls_addr, 16) == 0x0))
                    {
                        FetchTLSAddr(offset);
                    }

                    pe_stack_commit = FetchImageHeaderOffset(0x0130, 0x0004, 0);
                    t.AppendText("PE Stack Commit: 0x" + pe_stack_commit + "\r\n");

                    pe_heap_reserv = FetchImageHeaderOffset(0x0134, 0x0004, 0);
                    t.AppendText("PE Heap Reserve: 0x" + pe_heap_reserv + "\r\n");

                    pe_heap_commit = FetchImageHeaderOffset(0x0138, 0x0004, 0);
                    t.AppendText("PE Heap Commit: 0x" + pe_heap_commit + "\r\n");

                    pe_base_addr = FetchImageHeaderOffset(0x013c, 0x0004, 0);
                    t.AppendText("PE Base Address: 0x" + pe_base_addr + "\r\n");

                    pe_size_image = FetchImageHeaderOffset(0x0140, 0x0004, 0);
                    t.AppendText("PE Size of Image: 0x" + pe_size_image + "\r\n");

                    pe_checksum = FetchImageHeaderOffset(0x0144, 0x0004, 0);
                    t.AppendText("PE Checksum: 0x" + pe_checksum + "\r\n");

                    pe_timedata = FetchImageHeaderOffset(0x0148, 0x0004, 0);
                    t.AppendText("PE TimeDate: 0x" + pe_timedata);

                    if (!(Convert.ToInt32(pe_timedata, 16) == 0x0)) { 
                        FetchDateTime(Convert.ToInt32(pe_timedata, 16)); 
                    }

                    debug_pathname_addr = FetchImageHeaderOffset(0x014c, 0x0004, 0);
                    t.AppendText("\r\n" + "Debug PathName Address: 0x" + debug_pathname_addr);

                    //Subtract Pathname with Base Address to retrive the directory pathname.
                    offset = SubtractOffset(debug_pathname_addr, base_addr);
                    if (!(debug_pathname_addr == null)) {
                        t.AppendText(" (0x" + offset.ToString("X8") + ")" + "\r\n");
                    }

                    final_debug_pathname_addr = FetchImageHeaderOffset(offset, 0x0004, 1);
                    if (!(final_debug_pathname_addr == null)) {
                        t.AppendText(" (" + final_debug_pathname_addr + ")" + "\r\n"); 
                    }

                    debug_filename_addr = FetchImageHeaderOffset(0x0150, 0x0004, 0);
                    t.AppendText("Debug FileName Address: 0x" + debug_filename_addr);

                    //Subtract Debug Filename with Base Address to retrive the directory pathname.
                    offset = SubtractOffset(debug_filename_addr, base_addr);
                    if (!(debug_filename_addr == null)) {
                        t.AppendText(" (0x" + offset.ToString("X8") + ")" + "\r\n"); 
                    }
                    
                    final_debug_filename_addr = FetchImageHeaderOffset(offset, 0x0004, 1);
                    
                    if (!(final_debug_filename_addr == null)) {
                        t.AppendText(" (" + final_debug_filename_addr + ")" + "\r\n"); 
                    }

                    unicode_filename_addr = FetchImageHeaderOffset(0x0154, 0x0004, 0);
                    t.AppendText("Debug Unicode FileName Address: 0x" + unicode_filename_addr);

                    //Subtract Debug Unicode Filename with Base Address to retrive the directory pathname.
                    offset = SubtractOffset(unicode_filename_addr, base_addr);
                    
                    if (!(unicode_filename_addr == null)) {
                        t.AppendText(" (0x" + offset.ToString("X8") + ")"); 
                    } 
                    
                    final_unicode_filename_addr = FetchImageHeaderOffset(offset, 0x0004, 3);
                    
                    if (!(final_unicode_filename_addr == null))
                    {
                        t.AppendText(" L" + "\"" + final_unicode_filename_addr + "\"");//"\"" Allows for quotations
                    }
       
                    kernel_thunk_addr = FetchImageHeaderOffset(0x0158, 0x0004, 0);
                    t.AppendText("\r\n" + "Kernel Image Thunk Address: 0x" + kernel_thunk_addr);

                    FetchKernelThunk(Convert.ToInt32(kernel_thunk_addr, 16));

                    non_kernel_dir_addr = FetchImageHeaderOffset(0x015c, 0x0004, 0);
                    t.AppendText("Non-Kernel Import Directory Address: 0x" + non_kernel_dir_addr + "\r\n");

                    num_lib_versions = FetchImageHeaderOffset(0x0160, 0x0004, 0);
                    t.AppendText("Number of Library Versions: 0x" + num_lib_versions);
                    
                    if (Convert.ToInt32(num_lib_versions, 16) > 1)
                    {
                        t.AppendText(" (" + Convert.ToInt32(num_lib_versions, 16).ToString("D") + " Libraries)");
                    }
                    else if (Convert.ToInt32(num_lib_versions, 16) == 1)
                    {
                        t.AppendText(" (" + Convert.ToInt32(num_lib_versions, 16).ToString("D") + " Library)");
                    }
                    
                    lib_vers_addr = FetchImageHeaderOffset(0x0164, 0x0004, 0);
                    t.AppendText("\r\n" + "Library Versions Address: 0x" + lib_vers_addr);

                    //Subtract Library Versions Address with Base Address to retrive the Library Version.
                    offset = SubtractOffset(lib_vers_addr, base_addr);
                    if (!(lib_vers_addr == null))
                    {
                        t.AppendText(" (0x" + offset.ToString("X8") + ")" + "\r\n");
                    }
                    if (!(offset == 0x0))
                    {
                        FetchLibraryVersion(offset);
                    }

                    kernel_lib_vers_addr = FetchImageHeaderOffset(0x0168, 0x0004, 0);
                    t.AppendText("Kernel Library Version Address: 0x" + kernel_lib_vers_addr);
                    
                    //Subtract Kernel Library Version Address with Base Address to retrive the Kernel Library Version.
                    offset = SubtractOffset(kernel_lib_vers_addr, base_addr);
                    if (!(kernel_lib_vers_addr == null))
                    {
                        t.AppendText(" (0x" + offset.ToString("X8") + ")" + "\r\n");
                    }
                    if (!(offset == 0x0))
                    {
                        FetchKernelLibraryVersion(offset);
                    }

                    xapi_lib_vers_addr = FetchImageHeaderOffset(0x016c, 0x0004, 0);
                    t.AppendText("XAPI Library Version Address: 0x" + xapi_lib_vers_addr);

                    //Subtract XAPI Library Version Address with Base Address to retrive the XAPI Library Version.
                    offset = SubtractOffset(xapi_lib_vers_addr, base_addr);
                    if (!(xapi_lib_vers_addr == null))
                    {
                        t.AppendText(" (0x" + offset.ToString("X8") + ")" + "\r\n");
                    }
                    
                    if (!(offset == 0x0)) { 
                        FetchXAPIVersion(offset); 
                    }

                    //Logo Bitmap Address, where the Microsoft Logo can be found.
                    logo_bitmap_addr = FetchImageHeaderOffset(0x0170, 0x0004, 0);
                    t.AppendText("Logo Bitmap Address: 0x" + logo_bitmap_addr);
                    
                    //Subtract the logo Bitmap Address with the Base Address to obtain the correct Bitmap Address.
                    offset = SubtractOffset(logo_bitmap_addr, base_addr);
                    if (!(offset == 0x0)) {
                        t.AppendText(" (0x" + offset.ToString("X8") + ")"); 
                    }
                                   
                    //The Overall size to our Bitmap Logo.
                    logo_bitmap_size = FetchImageHeaderOffset(0x0174, 0x0004, 0);
                    t.AppendText("\r\n" +"Logo Bitmap Size: 0x" + logo_bitmap_size + "\r\n\r\n");
                    //FetchBitmap(Offset, Convert.ToInt32(logo_bitmap_size, 16));

                }
                else
                {
                    t.AppendText("Not a valid Xbox Executable" + "\r\n");
                    pXbe.Close();
                }
            }
            catch (Exception)
            {
                RichTextBox t = Application.OpenForms["frmApp"].Controls["textBox1"] as RichTextBox;
                t.AppendText("Error Occurred at Offset: 0x" + offset.ToString("X") + "\r\n");
                offset = 0x0;
                pXbe.Close(); 
            }
        }
    }
}
