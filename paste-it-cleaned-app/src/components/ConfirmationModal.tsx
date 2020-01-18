import React from 'react'

import Backdrop from '@material-ui/core/Backdrop'
import Fade from '@material-ui/core/Fade'

import { ModalProps } from '@material-ui/core/Modal'
import { Button, Modal, Typography } from '@material-ui/core'

import './Modal.less'
import './ConfirmationModal.less'

export type ConfirmationModalProps = ModalProps & {
    title: string
    onCancel: () => void
    onConfirm: () => void
}

class ConfirmationModal extends React.Component<ConfirmationModalProps> {
    render() {
        return (
            <Modal
                open={this.props.open}
                onClose={this.props.onCancel}
                closeAfterTransition
                BackdropComponent={Backdrop}
                BackdropProps={{
                    timeout: 500,
                }}
            >
                <Fade in={this.props.open}>
                    <div className={this.props.className ? `modal confirmation-modal ${this.props.className}` : 'modal confirmation-modal'}>
                        <div className="modal-header confirmation-modal-header">
                            <Typography variant="h2" component="h2">
                                {this.props.title}
                            </Typography>
                        </div>
                        <div className="modal-body confirmation-modal-body">{this.props.children}</div>
                        <div className="modal-buttons confirmation-modal-buttons">
                            <Button variant="contained" onClick={this.props.onCancel}>
                                No
                            </Button>
                            <Button variant="contained" color="primary" onClick={this.props.onConfirm}>
                                Yes
                            </Button>
                        </div>
                    </div>
                </Fade>
            </Modal>
        )
    }
}

export default ConfirmationModal
