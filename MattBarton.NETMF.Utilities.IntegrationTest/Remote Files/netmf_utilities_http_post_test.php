<?php

// Test file for integration testing HTTP GET requests

echo "HTTP POST Request successful. Post data arguments are: ";

foreach ($_POST as $key => $value)
{
	echo $key . ": " . $value . ", ";
}

?>