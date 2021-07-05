<?php
 if(isset($_GET["postcode"]) && !empty($_GET["postcode"]))
 {
     $mapapikey = get_field("google_map_api_key",'options');
     $zip_or_city = $_POST['postcode'];
     $zip_or_city_search = $zip_or_city.' Australia';
    
     $url2 = "https://maps.googleapis.com/maps/api/geocode/json?address=".urlencode($zip_or_city_search)."&key=".$mapapikey."&region=AUS";
     $result_string2 = file_get_contents($url2);
     $result2 = json_decode($result_string2, true);
     $lat2 = $result2['results'][0]['geometry']['location']['lat'];
     $lan2 = $result2['results'][0]['geometry']['location']['lng']; 
     $radius = 20;

     $args_locations	= array(
         'post_type' => 'product',
         'posts_per_page' => -1,
         'orderby' => 'ID',
         'order' => 'ASC',
         
       );
     $loop = new WP_Query( $args_locations );
     $i=0;
    
     if ( $loop->have_posts() ) :
     while ( $loop->have_posts() ) : $loop->the_post();

         $latitude = get_post_meta($post->ID, "location", true);
         $lat = $latitude['lat'];
         $lan = $latitude['lng'];
             
         $theta = $lan - $lan2;
         $dist = sin(deg2rad((double)$lat)) * sin(deg2rad((double)$lat2)) + cos(deg2rad((double)$lat)) * cos(deg2rad((double)$lat2)) * cos(deg2rad((double)$theta));
         $dist = acos($dist);
         $dist = rad2deg($dist);
         $distance = round($dist * 60 * 1.1515 * 1.609344);
         
             if($radius >= $distance)
             {
                 $ary_id[$i]['actid'] = $post->ID;
                 $ary_id[$i]['distance'] = $distance;
                 $i++;
             }
            else
             {
                 $ary_id[$i]['actid'] = $post->ID;
                 $ary_id[$i]['distance'] = $distance;
                 $i++;
             }
         

     endwhile; wp_reset_query(); wp_reset_postdata();
     endif;
     array_multisort(array_column($ary_id, 'distance'), SORT_ASC, $ary_id);
     foreach($ary_id as $pids)
     {
         array_push($act_id, $pids['actid']);
     }
     $postcode=1;