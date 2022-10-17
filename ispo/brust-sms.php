<?php

$curl = curl_init();
$username = '{USERNAME}';
$password = '{PASSWORD}';
$message = '{YOUR_MESSAGE}';

$authkey = base64_encode("$username:$password");
curl_setopt_array($curl, array(
CURLOPT_URL => 'https://api.transmitsms.com/send-sms.json',
CURLOPT_RETURNTRANSFER => true,
CURLOPT_ENCODING => '',
CURLOPT_MAXREDIRS => 10,
CURLOPT_TIMEOUT => 0,
CURLOPT_FOLLOWLOCATION => true,
CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
CURLOPT_CUSTOMREQUEST => 'POST',
CURLOPT_POSTFIELDS => 'message='. urlencode($message) .'&to={PHONE_NUMBER}&countrycode={COUNTRY_CODE}',
CURLOPT_HTTPHEADER => array(
    'Authorization: Basic '.$authkey ,
    'Content-Type: application/x-www-form-urlencoded'
),
));

$response = curl_exec($curl);
curl_close($curl);