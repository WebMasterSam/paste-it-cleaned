import React from 'react'

import Backdrop from '@material-ui/core/Backdrop'
import Fade from '@material-ui/core/Fade'

import { ModalProps } from '@material-ui/core/Modal'
import { Button, Modal, Typography } from '@material-ui/core'

import './Modal.less'
import './UpdateModal.less'

export type UpdateModalProps = ModalProps & {
    title: string
    onCancel: () => void
    onConfirm: () => void
    confirmActive: boolean
}

class UpdateModal extends React.Component<UpdateModalProps> {
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
                    <div className={this.props.className ? `modal udpate-modal ${this.props.className}` : 'modal udpate-modal'}>
                        <div className="modal-header udpate-modal-header">
                            <Typography variant="h2" component="h2">
                                {this.props.title}
                            </Typography>
                        </div>
                        <div className="modal-body udpate-modal-body">{this.props.children}</div>
                        <div className="modal-buttons udpate-modal-buttons">
                            <Button variant="contained" onClick={this.props.onCancel}>
                                Cancel
                            </Button>
                            <Button variant="contained" color="primary" onClick={this.props.onConfirm} disabled={!this.props.confirmActive}>
                                Save
                            </Button>
                        </div>
                    </div>
                </Fade>
            </Modal>
        )
    }
}

export default UpdateModal
