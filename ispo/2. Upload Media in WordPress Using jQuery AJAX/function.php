<?php 

// Single File Upload

add_action( 'wp_ajax_single_file_upload', 'single_file_upload' );
add_action( 'wp_ajax_nopriv_single_file_upload', 'single_file_upload' );

function single_file_upload()
{
  require( ABSPATH . 'wp-load.php' );
  $wordpress_upload_dir = wp_upload_dir();

  $file = $_FILES["file"];

  $centerpath = 1;
            
  $new_file_path = $wordpress_upload_dir['path'] . '/' . $file['name'];
  $new_file_mime = mime_content_type( $file['tmp_name'] );

  if( empty( $file ) ){
      die( 'File is not selected.' );
  }

  while( file_exists( $new_file_path ) ) {
      $centerpath++;
      $new_file_path = $wordpress_upload_dir['path'] . '/' . $centerpath  . '_' .$file['name'];
  }

  if( move_uploaded_file( $file['tmp_name'], $new_file_path ) ) {

      $upload_id = wp_insert_attachment( array(
          'guid'           => $new_file_path, 
          'post_mime_type' => $new_file_mime,
          'post_title'     => preg_replace( '/\.[^.]+$/', '', $file['name'] ),
          'post_content'   => '',
          'post_status'    => 'inherit'
      ), $new_file_path );

      require_once( ABSPATH . 'wp-admin/includes/image.php' );

      wp_update_attachment_metadata( $upload_id, wp_generate_attachment_metadata( $upload_id, $new_file_path ) );
  }     
  
  echo $upload_id;
  exit;
}


// Multi File Upload

add_action( 'wp_ajax_multi_file_upload', 'multi_file_upload' );
add_action( 'wp_ajax_nopriv_multi_file_upload', 'multi_file_upload' );

function multi_file_upload()
{
  require( ABSPATH . 'wp-load.php' );
  $wordpress_upload_dir = wp_upload_dir();

  $upload_id_array = array();

  for($i = 0; $i < count($_FILES['file']['name']); $i++) {
      

    $file = $_FILES["file"];

    $centerpath = 1;
              
    $new_file_path = $wordpress_upload_dir['path'] . '/' . $file['name'][$i];
    $new_file_mime = mime_content_type( $file['tmp_name'][$i] );

    if( empty( $file ) ){
        die( 'File is not selected.' );
    }

    while( file_exists( $new_file_path ) ) {
        $centerpath++;
        $new_file_path = $wordpress_upload_dir['path'] . '/' . $centerpath  . '_' .$file['name'][$i];
    }

    if( move_uploaded_file( $file['tmp_name'][$i], $new_file_path ) ) {

        $upload_id = wp_insert_attachment( array(
            'guid'           => $new_file_path, 
            'post_mime_type' => $new_file_mime,
            'post_title'     => preg_replace( '/\.[^.]+$/', '', $file['name'][$i] ),
            'post_content'   => '',
            'post_status'    => 'inherit'
        ), $new_file_path );

        require_once( ABSPATH . 'wp-admin/includes/image.php' );

        wp_update_attachment_metadata( $upload_id, wp_generate_attachment_metadata( $upload_id, $new_file_path ) );
        array_push( $upload_id_array,$upload_id);
    }     
  }
  echo json_encode($upload_id_array);
  exit;
}


?>