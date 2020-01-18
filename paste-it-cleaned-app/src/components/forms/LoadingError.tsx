import React, { Fragment, ReactNode } from 'react'
import ErrorOutlineIcon from '@material-ui/icons/ErrorOutline'

import './LoadingError.less'

export type LoadingErrorProps = {}

class LoadingError extends React.Component<LoadingErrorProps> {
    render() {
        return (
            <div className="loading-error">
                <ErrorOutlineIcon />
                <label>An error occured while loading the data.</label>
                <p>Please try again later.</p>
            </div>
        )
    }
}

export default LoadingError
