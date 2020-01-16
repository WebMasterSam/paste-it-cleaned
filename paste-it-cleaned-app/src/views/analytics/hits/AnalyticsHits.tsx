import React from 'react'

import { Paper, Typography } from '@material-ui/core'
import FileCopyIcon from '@material-ui/icons/FileCopy'

import FormWrapper from '../../../components/FormWrapper'

import HitsTable from './components/HitsTable'
import { Hit } from '../../../entities/api'
import { AnalyticsHitsController } from './AnalyticsHitsController'

import './AnalyticsHits.less'

export interface AnalyticsHitsProps {}
export interface AnalyticsHitsState {
    hits: Hit[]
    hitsLoading: boolean
    hitsError: boolean
}

class AnalyticsHits extends React.Component<AnalyticsHitsProps, AnalyticsHitsState> {
    private controller?: AnalyticsHitsController = undefined

    constructor(props: AnalyticsHitsProps) {
        super(props)
        this.controller = new AnalyticsHitsController(this)
        this.state = {
            hits: [],
            hitsLoading: false,
            hitsError: false,
        }
    }

    componentDidMount() {
        this.controller!.initialize()
    }

    render() {
        return (
            <Paper className="paper wide">
                <Typography variant="h2" className="override-h2" component="h2">
                    <FileCopyIcon /> Last plugin uses
                </Typography>

                <FormWrapper>
                    <HitsTable loading={this.state.hitsLoading} error={this.state.hitsError} rows={this.state.hits} full={true} />
                </FormWrapper>
            </Paper>
        )
    }
}

export default AnalyticsHits
