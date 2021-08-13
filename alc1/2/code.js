const urlMetadata = require('url-metadata');
//get matadata with use of website link.
router.put('/lastUrl', async (req, res) => {
    Logger.log.info('in url get API call');
    let lastUrlClicked;
    if (req.body.lastUrlClicked) {
        lastUrlClicked = req.body.lastUrlClicked;
    }
    try {
        let urlTitle = await urlMetadata(lastUrlClicked);
        res.status(200).send(urlTitle);
    } catch (err) {
        Logger.log.error('Error getting in metaData');
        Logger.log.error(err.message || err);
        return res.status(500).send({ message: 'Something went wrong, please try again later.' });
    }
});