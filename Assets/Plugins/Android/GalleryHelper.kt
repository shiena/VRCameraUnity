package photocamera

import android.content.ContentValues
import android.os.Build
import android.os.Environment
import android.provider.MediaStore
import java.io.File
import java.nio.file.Files

object GalleryHelper {
    @JvmStatic
    fun saveImage(path: String, data: ByteArray) {
        val f = File(Environment.DIRECTORY_PICTURES, path)
        val imageName = f.name
        val mimeType = Files.probeContentType(f.toPath());
        val contentValues = ContentValues().apply {
            put(MediaStore.Images.Media.DISPLAY_NAME, imageName)
            put(MediaStore.Images.Media.MIME_TYPE, mimeType)
            put(MediaStore.Images.Media.TITLE, imageName)
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
                val relPath = f.parent ?: ""
                put(MediaStore.Images.Media.RELATIVE_PATH, relPath)
                put(MediaStore.Images.Media.IS_PENDING, true)
            }
        }
        val context = com.unity3d.player.UnityPlayer.currentActivity
        val contentResolver = context.contentResolver
        val externalStorageUri = if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
            MediaStore.Images.Media.getContentUri(MediaStore.VOLUME_EXTERNAL_PRIMARY)
        } else {
            MediaStore.Images.Media.EXTERNAL_CONTENT_URI
        }
        val imageUri = contentResolver.insert(externalStorageUri, contentValues)

        imageUri?.let {
            contentResolver.openOutputStream(it).use { out ->
                out?.write(data)
            }
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
                contentValues.clear()
                contentValues.put(MediaStore.Images.Media.IS_PENDING, false)
                contentResolver.update(it, contentValues, null, null)
            }
        }
    }
}
