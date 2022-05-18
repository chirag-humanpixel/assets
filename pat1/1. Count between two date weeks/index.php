<?php 

$date1 = "2022-08-06";
$date2 = "2022-06-02" ;
$first_basic = date('m/d/Y',strtotime($date1));
$second_basic = date('m/d/Y',strtotime($date2));

$first = DateTime::createFromFormat('m/d/Y',$first_basic);
$second = DateTime::createFromFormat('m/d/Y',$second_basic);

$total_week = floor($second->diff($first)->days/7);
?>