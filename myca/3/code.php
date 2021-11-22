<?php use Box\Spout\Reader\Common\Creator\ReaderEntityFactory;

require_once $_SERVER['DOCUMENT_ROOT'] . '/dataimport/Spout/Autoloader/autoload.php';
require_once $_SERVER['DOCUMENT_ROOT'] . '/wp-load.php';

require_once ABSPATH . 'wp-admin/includes/media.php';
require_once ABSPATH . 'wp-admin/includes/file.php';
require_once ABSPATH . 'wp-admin/includes/image.php';

$reader = ReaderEntityFactory::createXLSXReader();
$filePath = $_SERVER['DOCUMENT_ROOT'] . '/dataimport/importvendors-only.xlsx';
$reader->open($filePath);

foreach ($reader->getSheetIterator() as $sheet) {
    foreach ($sheet->getRowIterator() as $len => $row) {
        if ($len > 1) {
            $rowAsArray = $row->toArray();

            /** import vendor */
            $storeName = $rowAsArray['1']; //
            $storeslugName = sanitize_title($rowAsArray['2']); //
            $Email = $rowAsArray['3']; //
            $servicetype = $rowAsArray['4'];
            //$providertypr = $rowAsArray['6'];
            $IsNDISProvider = $rowAsArray['6'];
            $firstname = $rowAsArray['7']; //
            $lastname = $rowAsArray['8']; //
            $addr1 = $rowAsArray['9']; //
            $addr2 = $rowAsArray['10']; //
            $city = $rowAsArray['11']; //
            $state = $rowAsArray['12']; //
            $Postcode = $rowAsArray['13']; //
            $country = "AU"; //
            $storephone = $rowAsArray['14'];
            $website = $rowAsArray['15'];
            $tellusservice = $rowAsArray['16'];
            $shortdesc = $rowAsArray['17'];
            $cologo = $rowAsArray['18'];
            $cbanner = $rowAsArray['19'];

            if (email_exists($Email)) {

                $pwd = substr($Email, 0, 8);

                $user = get_user_by('email', $Email);
                if ($user) {
                    $user_id = $user->ID;
                    wp_update_user(array('ID' => $user_id, 'user_nicename' => $storeslugName));

                    $uplaoddir = wp_upload_dir();

                    $logopath = $uplaoddir['basedir'] . '/2021/10/' . $cologo;
                    $logolink = $uplaoddir['baseurl'] . '/2021/10/' . $cologo;
                    $attach_id_logo = 0;
                    if (file_exists($logopath)) {
                        $filetype = wp_check_filetype(basename($logopath), null);
                        $attachment = array(
                            'guid' => $uplaoddir['url'] . '/' . basename($logopath),
                            'post_mime_type' => $filetype['type'],
                            'post_title' => preg_replace('/\.[^.]+$/', '', basename($logopath)),
                            'post_content' => '',
                            'post_status' => 'inherit',
                        );

                        // Insert the attachment.
                        $attach_id_logo = wp_insert_attachment($attachment, $logopath, 0);

                        // Generate the metadata for the attachment, and update the database record.
                        $attach_data = wp_generate_attachment_metadata($attach_id, $logopath);
                        wp_update_attachment_metadata($attach_id_logo, $attach_data);
                    }

                    $bannerpath = $uplaoddir['basedir'] . '/2021/10/' . $cbanner;
                    $bannerlink = $uplaoddir['baseurl'] . '/2021/10/' . $cbanner;
                    $attach_id_banner = 0;
                    if (file_exists($bannerpath)) {
                        $filetype = wp_check_filetype(basename($bannerpath), null);
                        $attachment = array(
                            'guid' => $uplaoddir['url'] . '/' . basename($bannerpath),
                            'post_mime_type' => $filetype['type'],
                            'post_title' => preg_replace('/\.[^.]+$/', '', basename($bannerpath)),
                            'post_content' => '',
                            'post_status' => 'inherit',
                        );

                        // Insert the attachment.
                        $attach_id_banner = wp_insert_attachment($attachment, $bannerpath, 0);

                        // Generate the metadata for the attachment, and update the database record.
                        $attach_data = wp_generate_attachment_metadata($attach_id, $bannerpath);
                        wp_update_attachment_metadata($attach_id_banner, $attach_data);
                    }

                    $street_number = '';
                    $street_name = '';
                    $street_name_short = '';
                    $city = '';
                    $state = '';
                    $state_short = '';
                    $post_code = '';
                    $country = '';
                    $country_short = '';

                    if (!empty($location)) {

                        $url2 = "https://maps.googleapis.com/maps/api/geocode/json?address=" . urlencode($location) . "&key=" . $mapapikey . "&region=AUS";
                        $result_string2 = file_get_contents($url2);
                        $result2 = json_decode($result_string2, true);

                        $address = $result2['results']['0']['formatted_address'];
                        $lat = $result2['results']['0']['geometry']['location']['lat'];
                        $lng = $result2['results']['0']['geometry']['location']['lng'];
                        $place_id = $result2['results']['0']['place_id'];

                        $addresscomp = $result2['results']['0']['address_components'];

                        foreach ($addresscomp as $key => $value) {
                            if (in_array("subpremise", $value['types'])) {$street_number .= $value['long_name'] . '/';}
                            if (in_array("street_number", $value['types'])) {$street_number .= $value['long_name'];}
                            if (in_array("route", $value['types'])) {$street_name .= $value['long_name'];
                                $street_name_short .= $value['short_name'];}
                            if (in_array("locality", $value['types'])) {$city .= $value['long_name'];}
                            if (in_array("administrative_area_level_1", $value['types'])) {$state .= $value['long_name'];
                                $state_short .= $value['short_name'];}
                            if (in_array("postal_code", $value['types'])) {$post_code .= $value['long_name'];}
                            if (in_array("country", $value['types'])) {$country .= $value['long_name'];
                                $country_short .= $value['short_name'];}
                        }

                        $metaval = array(
                            'address' => $address,
                            'lat' => $lat,
                            'lng' => $lng,
                            'zoom' => '12',
                            'place_id' => $place_id,
                            'street_number' => $street_number,
                            'street_name' => $street_name,
                            'street_name_short' => $street_name_short,
                            'city' => $city,
                            'state' => $state,
                            'state_short' => $state_short,
                            'post_code' => $post_code,
                            'country' => $country,
                            'country_short' => $country_short,
                        );
                    }

                    $custominfo = array('autism-friendly' => $servicetype, 'website-address' => $website, 'tell-us-about-your-services' => $tellusservice);
                    $serializevalue = array('address' => array('addr_1' => $addr1, 'addr_2' => $addr2, 'country' => 'AU', 'city' => $city, 'state' => $state, 'zip' => $Postcode), 'phone' => $storephone);
                    $wcfmmp_profile_settings = array('store_name' => $storeName, 'shop_description' => $shortdesc, 'phone' => $storephone, 'show_email' => 'no', 'store_name_position' => 'on_header', 'banner' => $attach_id_banner, 'store_logo' => $attach_id_logo, 'gravatar' => $attach_id_logo, 'store_slug' => $storeslugName, 'list_banner_type' => 'single_img', 'banner_type' => 'single_img', 'customer_support' => array('email' => $Email, 'address1' => $addr1, 'address2' => $addr2, 'country' => $country, 'city' => $city, 'state' => $state, 'zip' => $Postcode, 'phone' => $storephone), 'address' => array('street_1' => $addr1m, 'street_2' => $addr2, 'country' => $country, 'city' => $city, 'state' => $state, 'zip' => $Postcode));

                    $metas = array(
                        'first_name' => $firstname,
                        'last_name' => $lastname,
                        'billing_first_name' => $firstname,
                        'billing_last_name' => $lastname,
                        'shipping_first_name' => $firstname,
                        'shipping_last_name' => $lastname,
                        'billing_address_1' => $addr1,
                        'billing_address_2' => $addr2,
                        '_store_description' => $shortdesc,
                        'billing_country' => 'AU',
                        'billing_city' => $city,
                        'billing_state' => $state,
                        'billing_postcode' => $Postcode,
                        'shipping_address_1' => $addr1,
                        'shipping_address_2' => $addr2,
                        'shipping_country' => 'AU',
                        'shipping_city' => $city,
                        'shipping_state' => $state,
                        'shipping_postcode' => $Postcode,
                        'wcfmmp_store_name' => $storeName,
                        'store_name' => $storeName,
                        '_wcfmmp_profile_id' => $user_id,
                        'wcemailverified' => true,
                        '_wcfm_email_verified_for' => $Email,
                        '_wcfm_email_verified' => 1,
                        '_wcfmmp_profile_id' => $user_id,
                        'autism-friendly' => $servicetype,
                        'wcfmvm_custom_infos' => $custominfo,
                        'wcfmvm_static_infos' => $serializevalue,
                        'wcfmmp_profile_settings' => $wcfmmp_profile_settings,
                        'ndis_registered' => strtolower($IsNDISProvider),
                        'free_member' => 'yes',
                    );
                    foreach ($metas as $key => $value) {
                        update_user_meta($user_id, $key, $value);
                    }
                }
                //if($len>2){exit;}
            }
            /** import vdor */
        }
    }
}