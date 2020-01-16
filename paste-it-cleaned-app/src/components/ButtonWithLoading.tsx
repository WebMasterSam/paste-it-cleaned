import React from 'react'

import { CircularProgress } from '@material-ui/core'
import Button, { ButtonProps } from '@material-ui/core/Button'

import './ButtonWithLoading.less'

export type ButtonWithLoadingProps = ButtonProps & {
    loading: boolean
}

class ButtonWithLoading extends React.Component<ButtonWithLoadingProps> {
    render() {
        return (
            <Button {...this.props} disabled={this.props.loading} className="button-with-loading">
                {this.props.loading && <CircularProgress />}
                {this.props.children}
            </Button>
        )
    }
}

export default ButtonWithLoading
