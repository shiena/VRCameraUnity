package photocamera

import android.content.Context
import android.content.ContentValues
import android.content.Intent
import android.net.Uri
import android.os.Environment
import android.provider.MediaStore
import java.io.File
import java.nio.file.Files

object GalleryHelper {
    @JvmStatic
    fun getExternalStorageDirectory(): String {
        return Environment.getExternalStorageDirectory().absolutePath
    }

    @JvmStatic
    fun registerImage(path: String) {
        val f = File(path)
        val imageName = f.getName()
        val mimeType = Files.probeContentType(f.toPath());
        val contentValues = ContentValues().apply {
            put(MediaStore.Images.Media.MIME_TYPE, mimeType)
            put(MediaStore.Images.Media.DATA, path)
            put(MediaStore.Images.Media.TITLE, imageName)
            put(MediaStore.Images.Media.DISPLAY_NAME, imageName)
            put(MediaStore.Images.Media.DATE_ADDED, System.currentTimeMillis() / 1000)
        }
        val context = com.unity3d.player.UnityPlayer.currentActivity
        val contentResolver = context.getContentResolver()
        contentResolver.insert(MediaStore.Images.Media.EXTERNAL_CONTENT_URI, contentValues)

        // Refresh the Gallery
        val mediaScanIntent = Intent(Intent.ACTION_MEDIA_SCANNER_SCAN_FILE)
        mediaScanIntent.setData(Uri.fromFile(f))
        context.sendBroadcast(mediaScanIntent)
    }
}