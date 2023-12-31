﻿//https://nmaier.github.io/simpleDLNA/

using System;
using System.Collections.Generic;
using System.Linq;

namespace UpnpLib.Devices.Services.Media.Dlna
{
	public static class DlnaMaps
	{
		private static readonly string[] ext3GPP =
		  new string[] { "3gp", "3gpp" };

		private static readonly string[] extAAC =
		  new string[] { "aac", "mp4a", "m4a" };

		private static readonly string[] extAVC =
		  new string[] { "avc", "mp4", "m4v", "mov" };

		private static readonly string[] extAVI =
		  new string[] { "avi", "divx", "xvid" };

		private static readonly string[] extFLV =
		  new string[] { "flv" };

		private static readonly string[] extGIF =
		  new string[] { "gif" };

		private static readonly string[] extJPEG =
		  new string[] { "jpg", "jpe", "jpeg", "jif", "jfif" };

		private static readonly string[] extMATROSKA =
		  new string[] { "mkv", "matroska", "mk3d", "webm" };

		private static readonly string[] extMP3 =
		  new string[] { "mp3", "mp3p", "mp3x", "mp3a", "mpa" };

		private static readonly string[] extMP2 =
		  new string[] { "mp2" };

		private static readonly string[] extMPEG =
		  new string[] { "mpg", "mpe", "mpeg", "mpg2", "mpeg2", "ts", "vob", "m2v" };

		private static readonly string[] extOGV =
		  new string[] { "ogm", "ogv" };

		private static readonly string[] extPNG =
		  new string[] { "png" };

		private static readonly string[] extRAWAUDIO =
		  new string[] { "wav" };

		private static readonly string[] extVORBIS =
		  new string[] { "ogg", "oga" };

		private static readonly string[] extWMV =
		  new string[] { "wmv", "asf", "wma", "wmf" };

		private static readonly string[] extFLAC =
		  new string[] { "flac" };

		public static readonly Dictionary<DlnaMime, List<string>> Dlna2Ext =
		  new Dictionary<DlnaMime, List<string>>();

		public static readonly Dictionary<string, DlnaMime> Ext2Dlna =
		  new Dictionary<string, DlnaMime>();

		public static readonly Dictionary<string, DlnaMediaTypes> Ext2Media =
		  new Dictionary<string, DlnaMediaTypes>();

		public static readonly Dictionary<DlnaMediaTypes, List<string>> Media2Ext =
		  new Dictionary<DlnaMediaTypes, List<string>>();

		public static readonly Dictionary<DlnaMime, string> Mime = new Dictionary<DlnaMime, string>() {
		{ DlnaMime.AudioAAC, "audio/aac" },
		{ DlnaMime.AudioFLAC, "audio/flac" },
		{ DlnaMime.AudioMP2, "audio/mpeg" },
		{ DlnaMime.AudioMP3, "audio/mpeg" },
		{ DlnaMime.AudioRAW, "audio/L16;rate=44100;channels=2" },
		{ DlnaMime.AudioVORBIS, "audio/ogg" },
		{ DlnaMime.ImageGIF, "image/gif" },
		{ DlnaMime.ImageJPEG, "image/jpeg" },
		{ DlnaMime.ImagePNG, "image/png" },
		{ DlnaMime.SubtitleSRT, "smi/caption" },
		{ DlnaMime.Video3GPP, "video/3gpp" },
		{ DlnaMime.VideoAVC, "video/mp4" },
		{ DlnaMime.VideoAVI, "video/avi" },
		{ DlnaMime.VideoFLV, "video/flv" },
		{ DlnaMime.VideoMATROSKA, "video/x-mkv" },
		{ DlnaMime.VideoMPEG, "video/mpeg" },
		{ DlnaMime.VideoOGV, "video/ogg" },
		{ DlnaMime.VideoWMV, "video/x-ms-wmv" } };

		public static readonly Dictionary<DlnaMime, List<string>> AllPN = new Dictionary<DlnaMime, List<string>>() {
	  { DlnaMime.AudioAAC, new List<string> {
		"AAC"
	  } },
	  { DlnaMime.AudioFLAC, new List<string> {
		"FLAC"
	  } },
	  { DlnaMime.AudioMP2, new List<string> {
		"MP2_MPS"
	  } },
	  { DlnaMime.AudioMP3, new List<string> {
		"MP3"
	  } },
	  { DlnaMime.AudioRAW, new List<string> {
		"LPCM"
	  } },
	  { DlnaMime.AudioVORBIS, new List<string> {
		"OGG" } },
	  { DlnaMime.ImageGIF, new List<string> {
		"GIF",
		"GIF_LRG",
		"GIF_MED",
		"GIF_SM"
	  } },
	  { DlnaMime.ImageJPEG, new List<string> {
		"JPEG",
		"JPEG_LRG",
		"JPEG_MED",
		"JPEG_SM",
		"JPEG_TN"
	  } },
	  { DlnaMime.ImagePNG, new List<string> {
		"PNG",
		"PNG_LRG",
		"PNG_MED",
		"PNG_SM",
		"PNG_TN"
	  } },
	  { DlnaMime.SubtitleSRT, new List<string> {
		"SRT"
	  } },
	  { DlnaMime.Video3GPP, new List<string> {
		"MPEG4_P2_3GPP_SP_L0B_AMR",
		"AVC_3GPP_BL_QCIF15_AAC",
		"MPEG4_H263_3GPP_P0_L10_AMR",
		"MPEG4_H263_MP4_P0_L10_AAC",
		"MPEG4_P2_3GPP_SP_L0B_AAC" } },
	  { DlnaMime.VideoAVC, new List<string> {
		"AVC_MP4_MP_SD_AAC_MULT5",
		"AVC_MP4_HP_HD_AAC",
		"AVC_MP4_HP_HD_DTS",
		"AVC_MP4_LPCM",
		"AVC_MP4_MP_SD_AC3",
		"AVC_MP4_MP_SD_DTS",
		"AVC_MP4_MP_SD_MPEG1_L3",
		"AVC_TS_HD_50_LPCM_T",
		"AVC_TS_HD_DTS_ISO",
		"AVC_TS_HD_DTS_T",
		"AVC_TS_HP_HD_MPEG1_L2_ISO",
		"AVC_TS_HP_HD_MPEG1_L2_T",
		"AVC_TS_HP_SD_MPEG1_L2_ISO",
		"AVC_TS_HP_SD_MPEG1_L2_T",
		"AVC_TS_MP_HD_AAC_MULT5",
		"AVC_TS_MP_HD_AAC_MULT5_ISO",
		"AVC_TS_MP_HD_AAC_MULT5_T",
		"AVC_TS_MP_HD_AC3",
		"AVC_TS_MP_HD_AC3_ISO",
		"AVC_TS_MP_HD_AC3_T",
		"AVC_TS_MP_HD_MPEG1_L3",
		"AVC_TS_MP_HD_MPEG1_L3_ISO",
		"AVC_TS_MP_HD_MPEG1_L3_T",
		"AVC_TS_MP_SD_AAC_MULT5",
		"AVC_TS_MP_SD_AAC_MULT5_ISO",
		"AVC_TS_MP_SD_AAC_MULT5_T",
		"AVC_TS_MP_SD_AC3",
		"AVC_TS_MP_SD_AC3_ISO",
		"AVC_TS_MP_SD_AC3_T",
		"AVC_TS_MP_SD_MPEG1_L3",
		"AVC_TS_MP_SD_MPEG1_L3_ISO",
		"AVC_TS_MP_SD_MPEG1_L3_T" } },
	  { DlnaMime.VideoAVI, new List<string> {
		"AVI"
	  } },
	  { DlnaMime.VideoFLV, new List<string> {
		"FLV"
	  } },
	  { DlnaMime.VideoMATROSKA, new List<string> {
		"MATROSKA"
	  } },
	  { DlnaMime.VideoMPEG, new List<string> {
		"MPEG1",
		"MPEG_PS_PAL",
		"MPEG_PS_NTSC",
		"MPEG_TS_SD_EU",
		"MPEG_TS_SD_EU_T",
		"MPEG_TS_SD_EU_ISO",
		"MPEG_TS_SD_NA",
		"MPEG_TS_SD_NA_T",
		"MPEG_TS_SD_NA_ISO",
		"MPEG_TS_SD_KO",
		"MPEG_TS_SD_KO_T",
		"MPEG_TS_SD_KO_ISO",
		"MPEG_TS_JP_T" } },
	  { DlnaMime.VideoOGV, new List<string> {
		"OGV"
	  } },
	  { DlnaMime.VideoWMV, new List<string> {
		"WMV_FULL",
		"WMV_BASE",
		"WMVHIGH_FULL",
		"WMVHIGH_BASE",
		"WMVHIGH_PRO",
		"WMVMED_FULL",
		"WMVMED_BASE",
		"WMVMED_PRO",
		"VC1_ASF_AP_L1_WMA",
		"VC1_ASF_AP_L2_WMA",
		"VC1_ASF_AP_L3_WMA"
	  } }
	};

		public static readonly Dictionary<DlnaMime, string> MainPN = GenerateMainPN();

		public static readonly string ProtocolInfo = GenerateProtocolInfo();

		static DlnaMaps()
		{
			var e2d = new[] {
		new
	  { t = DlnaMime.AudioAAC, e = extAAC },
		new
	  { t = DlnaMime.AudioFLAC, e = extFLAC },
		new
	  { t = DlnaMime.AudioMP2, e = extMP2 },
		new
	  { t = DlnaMime.AudioMP3, e = extMP3 },
		new
	  { t = DlnaMime.AudioRAW, e = extRAWAUDIO },
		new
	  { t = DlnaMime.AudioVORBIS, e = extVORBIS },
		new
	  { t = DlnaMime.ImageGIF, e = extGIF },
		new
	  { t = DlnaMime.ImageJPEG, e = extJPEG },
		new
	  { t = DlnaMime.ImagePNG, e = extPNG },
		new
	  { t = DlnaMime.Video3GPP, e = ext3GPP },
		new
	  { t = DlnaMime.VideoAVC, e = extAVC },
		new
	  { t = DlnaMime.VideoAVI, e = extAVI },
		new
	  { t = DlnaMime.VideoFLV, e = extFLV },
		new
	  { t = DlnaMime.VideoMATROSKA, e = extMATROSKA },
		new
	  { t = DlnaMime.VideoMPEG, e = extMPEG },
		new
	  { t = DlnaMime.VideoOGV, e = extOGV },
		new
	  { t = DlnaMime.VideoWMV, e = extWMV }
	  };

			foreach (var i in e2d)
			{
				var t = i.t;
				foreach (var e in i.e)
				{
					Ext2Dlna.Add(e.ToUpperInvariant(), t);
				}
				Dlna2Ext.Add(i.t, new List<string>(i.e));
			}

			InitMedia(
			  new string[][] { ext3GPP, extAVI, extAVC, extFLV, extMATROSKA, extMPEG, extOGV, extWMV },
			  DlnaMediaTypes.Video);
			InitMedia(
			  new string[][] { extJPEG, extPNG, extGIF },
			  DlnaMediaTypes.Image);
			InitMedia(
			  new string[][] { extAAC, extFLAC, extMP2, extMP3, extRAWAUDIO, extVORBIS },
			  DlnaMediaTypes.Audio);
		}

		private static string GenerateProtocolInfo()
		{
			var pns = new List<string>();
			foreach (var p in AllPN)
			{
				var mime = Mime[p.Key];
				foreach (var pn in p.Value)
				{
					pns.Add(string.Format(
					  "http-get:*:{1}:DLNA.ORG_PN={0};DLNA.ORG_OP=01;DLNA.ORG_CI=0;DLNA.ORG_FLAGS={2}",
					  pn, mime, DefaultDlnaFlags.DefaultStreaming));
				}
			}
			return string.Join(",", pns);
		}

		private static void InitMedia(string[][] k, DlnaMediaTypes t)
		{
			foreach (var i in k)
			{
				var e = (from ext in i
						 select ext.ToUpperInvariant()).ToList();

				try
				{
					Media2Ext.Add(t, e);
				}
				catch (ArgumentException)
				{
					Media2Ext[t].AddRange(e);
				}
				foreach (var ext in e)
				{
					Ext2Media.Add(ext.ToUpperInvariant(), t);
				}
			}
		}

		public static Dictionary<DlnaMime, string> GenerateMainPN()
		{
			var rv = new Dictionary<DlnaMime, string>();
			foreach (var p in AllPN)
			{
				rv.Add(p.Key, p.Value.FirstOrDefault());
			}
			return rv;
		}
	}
}
