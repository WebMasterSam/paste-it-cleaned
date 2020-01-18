import React from 'react'

import { CircularProgress } from '@material-ui/core'
import Button, { ButtonProps } from '@material-ui/core/Button'

import './ButtonWithLoading.less'

export type ButtonWithLoadingProps = ButtonProps & {
    loading: boolean
}

class ButtonWithLoading extends React.Component<ButtonWithLoadingProps> {
    render() {
        var { children, loading, ...rest } = this.props
        return (
            <Button {...rest} disabled={loading} className="button-with-loading">
                {loading && <CircularProgress />}
                {children}
            </Button>
        )
    }
}

export default ButtonWithLoading
