<?php

function printr($val)
    {
        echo "<hr><pre>";
        print_r($val);
        echo "</pre><hr>";
    }

function phpWrapperFromFile($filename)
	{
		ob_start();
		
		if (file_exists($filename) && !is_dir($filename))
		{
			include($filename);
		}
		
		// nacte to z outputu
		$obsah = ob_get_clean();
		return $obsah;
	}

function FileUpload()
{
    $target_dir = "pdf/";
    $target_file = $target_dir . basename($_FILES["file"]["name"]);
    $uploadOk = 1;
    $imageFileType = pathinfo($target_file,PATHINFO_EXTENSION);
    // Check if image file is a actual image or fake image
    if(isset($_POST["submit"])) {
        $check = getimagesize($_FILES["file"]["tmp_name"]);
        if($check !== false) {
            echo "File is an image - " . $check["mime"] . ".";
            $uploadOk = 1;
        } else {
            echo "File is not an image.";
            $uploadOk = 0;
        }
    }
    
    // Check if file already exists
/*if (file_exists($target_file)) {
    echo "Sorry, file already exists. ";
    $uploadOk = 0;
}*/
// Check file size
/*if ($_FILES["file"]["size"] > 500000) {
    echo "Sorry, your file is too large. ";
    $uploadOk = 0;
}*/
// Allow certain file formats
if($imageFileType != "pdf" && $imageFileType != "txt") {
    echo "Sorry, only PDF or TXT files are allowed. ";
    $uploadOk = 0;
}
// Check if $uploadOk is set to 0 by an error
if ($uploadOk == 0) {
    echo "Sorry, your file was not uploaded. ";
    return false;
// if everything is ok, try to upload file
} else {
    if (move_uploaded_file($_FILES["file"]["tmp_name"], $target_file)) {
        echo "The file ". basename( $_FILES["file"]["name"]). " has been uploaded. ";
        return true;
    } else {
        echo "Sorry, there was an error uploading your file. ";
        return false;
    }
}
}

?>