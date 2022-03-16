<?php 
use Box\Spout\Writer\Common\Creator\WriterEntityFactory;
use Box\Spout\Common\Entity\Row;   

try {
	$filePath = getcwd().'/wp-content/themes/{child-theme}/csv/demo.csv'; // Use your custom path for where you want to generate CSV files.

	$writer = WriterEntityFactory::createCSVWriter();

	$writer->openToFile($filePath);

	// Here is data for XLSX file
	$data = [
		['Name', 'Email'],
		['Name1', 'Email1'],
		['Name2', 'Email2'],
	];
	
	foreach ($data as $d) {
		$cells = [
			WriterEntityFactory::createCell($d[0]),
			WriterEntityFactory::createCell($d[1]),
		];

		$singleRow = WriterEntityFactory::createRow($cells);
		$writer->addRow($singleRow);
	}

	$writer->close();
	
	
} catch(Exception $e) {
	echo $e->getMessage();
}