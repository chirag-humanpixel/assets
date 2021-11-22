<?php
function getDateForSpecificDayBetweenDates($startDate, $endDate, $day_number)
{
    $endDate = strtotime($endDate);
    $days = array('1' => 'Monday', '2' => 'Tuesday', '3' => 'Wednesday', '4' => 'Thursday', '5' => 'Friday', '6' => 'Saturday', '7' => 'Sunday');
    for ($i = strtotime($days[$day_number], strtotime($startDate)); $i <= $endDate; $i = strtotime('+1 week', $i)) {
        $date_array[] = date('Y-m-d', $i);
    }

    return $date_array;
}
$days = array('Monday' => '1', 'Tuesday' => '2', 'Wednesday' => '3', 'Thursday' => '4', 'Friday' => '5', 'Saturday' => '6', 'Sunday' => '7');
$date = "Monday";
$dnumber = $days[$date];
$dlist = getDateForSpecificDayBetweenDates($activty_start_date, $activty_end_date, $dnumber);