<?php

// Test file for integration testing HTTP GET requests

echo "HTTP GET Request successful. Querystring arguments are: ";

foreach ($_GET as $key => $value)
{
	echo $key . ": " . $value . ", ";
}

?>