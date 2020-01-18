import React from 'react'

import MuiAlert, { Color } from '@material-ui/lab/Alert'
import { Snackbar } from '@material-ui/core'

export type SnackProps = {
    visible: boolean
    onClose: () => void
    type: Color
}

class Snack extends React.Component<SnackProps> {
    render() {
        return (
            <Snackbar open={this.props.visible} autoHideDuration={4000} onClose={this.props.onClose}>
                <MuiAlert elevation={6} variant="filled" onClose={this.props.onClose} severity={this.props.type}>
                    {this.props.children}
                </MuiAlert>
            </Snackbar>
        )
    }
}

export default Snack
