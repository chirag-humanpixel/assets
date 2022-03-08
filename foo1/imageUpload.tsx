import { useEffect, useRef, useState } from 'react';
import dummy from '../../../src/assets/images/user-profile.svg';
import closeIcon from '../../assets/images/profile-close.svg';
import Button from '../button/Button';

type FileUploadProps = {
  fileName?: string;
  isProfile?: boolean;
  handleChange?: (e: any) => void;
  profilePictureUrl?: string;
  className?: string;
  file?: Blob | MediaSource;
  isDeleteIcon?: boolean;
  onDeleteClick?: () => void;
  removeFile: () => void;
};

const FileUpload = (props: FileUploadProps) => {
  const {
    isProfile = true,
    handleChange,
    profilePictureUrl,
    fileName,
    className,
    file,
    removeFile,
  } = props;

  const hiddenFileInput = useRef<HTMLInputElement>(null);

  const handleClick = () => {
    hiddenFileInput?.current?.click();
  };

  const [url, setUrl] = useState('');
  useEffect(() => {
    if (file) {
      setUrl(URL.createObjectURL(file));
    }
  }, [file]);

  const clearFileData = () => {
    setUrl('');
    removeFile();
  };

  return (
    <div className={className ?? 'user-dp-upload'}>
      {isProfile ? (
        <div className="profile-dp-upload">
          {profilePictureUrl || url ? (
            <div className="profile-dp-container">
              <img
                alt="profile-image"
                className="user-dp"
                src={url || profilePictureUrl || dummy}
              />
              {(url || profilePictureUrl) && (
                <button
                  type="button"
                  className="profile-close-btn"
                  onClick={clearFileData}
                >
                  <img src={closeIcon} />
                </button>
              )}
            </div>
          ) : (
            <img className="user-dp" src={dummy} />
          )}
        </div>
      ) : (
        ''
      )}
      <Button
        color="primary"
        variant="contained"
        className="secondary-button"
        onClick={handleClick}
      >
        Upload Photo
      </Button>

      <input
        type="file"
        className="input-file-type"
        ref={hiddenFileInput}
        onChange={handleChange}
      />
      <p onClick={handleClick}>{fileName}</p>
    </div>
  );
};

export default FileUpload;
